using DominosCutScreen.Server.Models;
using DominosCutScreen.Shared;

using System.Text.RegularExpressions;

namespace DominosCutScreen.Server.Services
{
    public class MakelineItemTransformer
    {
        private IServiceProvider _serviceProvider;

        public MakelineItemTransformer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T ProcessMakelineItem<T>(T item) where T : CommonMakelineItem
        {
            item.PrettyItemName = item.Description;
            EnsureBumpTimes(item);
            ParsePostBakes(item);
            TrimDescriptionDecorations(item);
            ParseCrustName(item);
            return item;
        }

        /// <summary>
        /// Sometimes the makeline will report a mis-match in the number of bump times vs quantity even though the order is bumped.
        /// This function adds more times to make up the difference.
        /// </summary>
        private void EnsureBumpTimes(CommonMakelineItem item)
        {
            if (item is MakeLineOrderLine makelineOrder)
            {
                // The old code used the order actual time but i cant be bothered adding an `Order` member and its not critical so `Now` will do.
                makelineOrder.BumpedTimes.AddRange(Enumerable.Range(0, makelineOrder.Quantity - makelineOrder.BumpedTimes.Count).Select(x => DateTime.Now));
            }
        }

        /// <summary>
        /// This will remove postbake indicators like <c>`*`</c> and <c>`+m`</c> and add them as topping modifications.
        /// If the postbake topping already exists in <paramref name="ToppingModifications"/> then it is ignored.
        /// </summary>
        private void ParsePostBakes(CommonMakelineItem item)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CutBenchContext>();

            var postbakes = new List<PostBake>();

            foreach (var str in item.PrettyItemName.Split(' '))
            {
                var postBakeIndex = str.IndexOfAny(new char[] { '+', '*' });

                // No post bakes, so do nothing
                if (postBakeIndex == -1)
                    continue;

                var postBakeStr = str[postBakeIndex..].Replace("+", string.Empty);

                foreach (var postbake in context.PostBakes.OrderByDescending(pair => pair.ReceiptCode.Length))
                {
                    if (postBakeStr.Contains(postbake.ReceiptCode, StringComparison.InvariantCultureIgnoreCase))
                    {
                        postBakeStr = postBakeStr.Replace(postbake.ReceiptCode, string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        postbakes.Add(postbake);
                    }
                }

                // Check if there is still postbake codes left
                if (postBakeStr.Length > 0)
                {
                    postbakes.Add(new ()
                    {
                        ToppingCode = "UKN",
                        ToppingDescription = $"Post bake: {postBakeStr}"
                    });
                }
            }

            item.PrettyItemName = Regex.Replace(item.PrettyItemName, @"[\*]|(:?\+[\w]+)+\b", string.Empty);

            var count = postbakes.Count;
            for (var i = 0; i < count; ++i)
            {
                // Clone so we can make changes without affecting the original reference
                var data = postbakes[i].AsToppingModification();
                var displaySeq = -count + i;

                // Topping code already exists in modifications so ignore this
                // This only bails if there is menu postbake modification on the whole pizza.
                if (item.ToppingModifications.Any(tm => tm.ToppingCode == data.ToppingCode && tm.PizzaDistribution == 87))
                    continue;

                // Work out the distribution for post bake changes only on one half.
                var distribution = 87;
                var existingToppingModification = item.ToppingModifications.Where(tm => tm.ToppingCode == data.ToppingCode).FirstOrDefault();

                if (existingToppingModification != null)
                {
                    distribution = existingToppingModification.PizzaDistribution switch
                    {
                        49 => 50,
                        50 => 49
                    };
                }

                data.DisplaySequence = displaySeq;
                data.PizzaDistribution = distribution;
                item.ToppingModifications.Add(data);
            }

            item.ToppingModifications = item.ToppingModifications.Where(tm =>
            {
                // If we dont have a postbake in the database OR the postbake in the database is enabled, allow it.
                // This filters out postbakes that are disabled in the database
                var pb = context.PostBakes.FirstOrDefault(pb => pb.ToppingCode == tm.ToppingCode);
                return pb == null || pb.IsEnabled;
            })
            .Select(tm =>
            {
                // Overwrite topping description with whats in the database

                var pb = context.PostBakes.FirstOrDefault(pb => pb.ToppingCode == tm.ToppingCode);

                // Only fix up toppings we didnt add as those have already been formatted
                if (pb != null && tm.DisplaySequence >= 0)
                {
                    tm.ToppingDescription = $"*** {pb.ToppingDescription}";
                }

                return tm;
            }).ToList();
        }

        /// <summary>
        /// Remove decorations like <c>`L `</c> and <c>`EA `</c> from <paramref name="Description" /> and store it in <paramref name="PrettyItemName" />
        /// </summary>
        private void TrimDescriptionDecorations(CommonMakelineItem item)
        {
            if (item.PrettyItemName.StartsWith("L ", StringComparison.InvariantCultureIgnoreCase))
            {
                item.PrettyItemName = item.PrettyItemName[2..];
            }
            else if (item.PrettyItemName.StartsWith("EA ", StringComparison.InvariantCultureIgnoreCase))
            {
                item.PrettyItemName = item.PrettyItemName[3..];
            }
            else if (item.PrettyItemName.StartsWith("CHK ", StringComparison.InvariantCultureIgnoreCase))
            {
                item.PrettyItemName = item.PrettyItemName[4..];
            }
        }

        /// <summary>
        /// This will parse crust names into something a lot nicer.
        /// e.g. <c>Mini DEEP Mini MDB => MDB</c>, <c>Mini DEEP => Mini</c>, <c>CLASS => Classic</c>
        /// </summary>
        private void ParseCrustName(CommonMakelineItem item)
        {
            // For full order items this is the difference between <paramref name="Description" /> and <paramref name="ProductDescription" />
            // For bumped items this is <paramref name="Description" />
            string? crustString = null;

            // Ignore sides, and oddly enough ignore MDB since their `ProductDescription` and `Description` are the same
            if (item is MakeLineOrderLine makelineItem && makelineItem.ProductCategory == "Pizza")
            {
                // For some stupid af reason, the double bacon cheeseburger has 2 different strings in `ProductDescription` and `Description`.
                //  "DblBcnChsBurg+m" and "Dbl Bcn Chs Burg+M"
                // My solution for this is the remove all the spaces from both, then remove then remove `ProductDescription` from `Description`.
                // The main issue with this is that crusts with spaces (e.g. "GLUTEN FREE") fail to get removed correctly in CommonMakelineItem.ParseCrustName.
                // the workaround is to create an empty pair in MakelineOverrideManager.CrustNameOverrides
                var desc = string.Concat(item.PrettyItemName.Where(c => !char.IsWhiteSpace(c)));
                string pdesc = string.Concat(makelineItem.ProductDescription.Where(c => !char.IsWhiteSpace(c)));
                crustString = desc.Replace(pdesc, null, StringComparison.InvariantCultureIgnoreCase);
            }

            // MDB `ProductDescription` and `Description` are the same, so just look for MDB
            if (crustString == null && !item.PrettyItemName.Contains("MDB", StringComparison.InvariantCultureIgnoreCase))
                return;

            var str = crustString ?? item.PrettyItemName;
            foreach (var pair in MakelineOverrideManager.CrustNameOverrides)
            {
                if (str.Contains(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    item.PizzaBaseName = pair.Value;
                    break;
                }
            }

            // Strip all crust keywords out of the item name
            item.PrettyItemName = string.Join(' ', item.PrettyItemName.Split(' ').Where(s => !MakelineOverrideManager.CrustNameOverrides.Keys.Any(k => k.Equals(s, StringComparison.InvariantCultureIgnoreCase))));
        }
    }
}
