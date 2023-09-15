using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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
            var postBakeIndex = PrettyItemName.IndexOfAny(new char[] { '+', '*' });

            // No post bakes, so do nothing
            if (postBakeIndex == -1)
                return;

            var hasSpringOnion = PrettyItemName.Contains("*");
            var postBakeStr = PrettyItemName[postBakeIndex..].Replace("+", null).Replace("*", null);
            PrettyItemName = PrettyItemName[0..postBakeIndex];
            // Reverse the list so that spring onion (which appears first) will be the last added topping modification
            var postbakes = postBakeStr.Split('+', StringSplitOptions.RemoveEmptyEntries).ToList();
            
            if (hasSpringOnion)
            {
                postbakes.Add("*");
            }

            var count = postbakes.Count();
            for (var i = 0; i < count; ++i)
            {
                var code = postbakes.ElementAt(i).ToUpper();
                var displaySeq = -count + i;

                if (MakelineOverrideManager.PostBakeOverrides.ContainsKey(code))
                {
                    var data = MakelineOverrideManager.PostBakeOverrides[code];

                    // Topping code already exists in modifications so ignore this
                    if (ToppingModifications.Any(tm => tm.ToppingCode == data.ToppingCode))
                        continue;

                    ToppingModifications.Add(new MakeLineToppingModification
                    {
                        DisplaySequence = displaySeq,
                        ToppingCode = data.ToppingCode,
                        ToppingDescription = $"*** {data.ToppingDescription}"
                    });
                }
                else
                {
                    Console.Error.WriteLine($"Unknown postbake code \"{code}\"");

                    ToppingModifications.Add(new MakeLineToppingModification
                    {
                        DisplaySequence = displaySeq,
                        ToppingCode = "UKN",
                        ToppingDescription = $"Post bake: {code}"
                    });
                }
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
