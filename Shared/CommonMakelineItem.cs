using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DominosCutScreen.Shared
{
    public class CommonMakelineItem
    {
        /// <summary>
        /// Name of the pizza or side.
        /// For pizzas, this will show up as `L CLASS Surpreme` or `L XL Classic Supreme` etc
        /// </summary>
        [XmlElement]
        public string Description { get; set; }

        /// <summary>
        /// No idea what this is for.
        /// Always seems to be 0
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public int LinePart { get; set; }

        /// <summary>
        /// List of all topping modifications
        /// </summary>
        [XmlArray, XmlArrayItem("MakeLineToppingModificationContract")]
        public List<MakeLineToppingModification> ToppingModifications { get; set; }

        [XmlIgnore]
        public string PrettyItemName { get; set; }

        [XmlIgnore]
        public string PizzaBaseName { get; set; }

        /// <summary>
        /// For full order items this is the difference between <paramref name="Description" /> and <paramref name="ProductDescription" />
        /// For bumped items this is <paramref name="Description" />
        /// </summary>
        [XmlIgnore]
        private string? _crustString { get; set; }

        /// <summary>
        /// Fix postbake indicator, remove description decorations and set the crust/base if applicable.
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="CrustString"></param>
        protected void OnDeserialized(string? CrustString = null)
        {
            PrettyItemName = Description;
            _crustString = CrustString;
            ParsePostBakes();
            TrimDescriptionDecorations();
            ParseCrustName();
        }

        /// <summary>
        /// This will remove postbake indicators like <c>`*`</c> and <c>`+m`</c> and add them as topping modifications.
        /// If the postbake topping already exists in <paramref name="ToppingModifications"/> then it is ignored.
        /// </summary>
        private void ParsePostBakes()
        {
            var postbakes = new List<MakeLineToppingModification>();

            foreach (var str in PrettyItemName.Split(' '))
            {
                var postBakeIndex = str.IndexOfAny(new char[] { '+', '*' });

                // No post bakes, so do nothing
                if (postBakeIndex == -1)
                    continue;

                var postBakeStr = str[postBakeIndex..].Replace("+", string.Empty);

                foreach (var (code, tm) in MakelineOverrideManager.PostBakeOverrides.OrderByDescending(pair => pair.Key.Length))
                {
                    if (postBakeStr.Contains(code, StringComparison.InvariantCultureIgnoreCase))
                    {
                        postBakeStr = postBakeStr.Replace(code, string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        postbakes.Add(tm);
                    }
                }

                // Check if there is still postbake codes left
                if (postBakeStr.Length > 0)
                {
                    postbakes.Add(new MakeLineToppingModification
                    {
                        ToppingCode = "UKN",
                        ToppingDescription = $"Post bake: {postBakeStr}",
                        PizzaDistribution = 87
                    });
                }
            }

            PrettyItemName = Regex.Replace(PrettyItemName, @"[\*]|(:?\+[\w]+)+\b", string.Empty);

            var count = postbakes.Count();
            for (var i = 0; i < count; ++i)
            {
                // Clone so we can make changes without affecting the original reference
                var data = postbakes[i].Clone();
                var displaySeq = -count + i;

                // Topping code already exists in modifications so ignore this
                // This only bails if there is menu postbake modification on the whole pizza.
                if (ToppingModifications.Any(tm => tm.ToppingCode == data.ToppingCode && tm.PizzaDistribution == 87))
                    continue;

                // Work out the distribution for post bake changes only on one half.
                var distribution = 87;
                var existingToppingModification = ToppingModifications.Where(tm => tm.ToppingCode == data.ToppingCode).FirstOrDefault();

                if (existingToppingModification != null)
                {
                    distribution = existingToppingModification.PizzaDistribution switch
                    {
                        49 => 50,
                        50 => 49
                    };
                }

                data.DisplaySequence = displaySeq;
                data.ToppingDescription = $"*** {data.ToppingDescription}";
                data.PizzaDistribution = distribution;
                ToppingModifications.Add(data);
            }
        }

        /// <summary>
        /// Remove decorations like <c>`L `</c> and <c>`EA `</c> from <paramref name="Description" /> and store it in <paramref name="PrettyItemName" />
        /// </summary>
        private void TrimDescriptionDecorations()
        {
            if (PrettyItemName.StartsWith("L ", StringComparison.InvariantCultureIgnoreCase))
            {
                PrettyItemName = PrettyItemName[2..];
            }
            else if (PrettyItemName.StartsWith("EA ", StringComparison.InvariantCultureIgnoreCase))
            {
                PrettyItemName = PrettyItemName[3..];
            }
            else if (PrettyItemName.StartsWith("CHK ", StringComparison.InvariantCultureIgnoreCase))
            {
                PrettyItemName = PrettyItemName[4..];
            }
        }

        /// <summary>
        /// This will parse crust names into something a lot nicer.
        /// e.g. <c>Mini DEEP Mini MDB => MDB</c>, <c>Mini DEEP => Mini</c>, <c>CLASS => Classic</c>
        /// </summary>
        void ParseCrustName()
        {
            // MDB `ProductDescription` and `Description` are the same, so just look for MDB
            if (_crustString == null && !PrettyItemName.Contains("MDB", StringComparison.InvariantCultureIgnoreCase))
                return;

            var str = _crustString ?? PrettyItemName;
            foreach (var pair in MakelineOverrideManager.CrustNameOverrides)
            {
                if (str.Contains(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    PizzaBaseName = pair.Value;
                    break;
                }
            }

            // Strip all crust keywords out of the item name
            PrettyItemName = string.Join(' ', PrettyItemName.Split(' ').Where(s => !MakelineOverrideManager.CrustNameOverrides.Keys.Any(k => k.Equals(s, StringComparison.InvariantCultureIgnoreCase))));
        }
    }
}
