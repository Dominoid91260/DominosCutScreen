using System.Xml.Serialization;

namespace DominosCutScreen.Shared
{
    public class MakeLineToppingModification : IEquatable<MakeLineToppingModification?>
    {
        /// <summary>
        /// Not sure what this is for. I'd guess that its for ordering modifications on the makeline
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public int DisplaySequence { get; set; }

        /// <summary>
        /// No idea what this is for.
        /// Seems to be `true` when a pizza topping is removed and `false` in every other situation
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public bool IsHvi { get; set; }

        /// <summary>
        /// Seems to be which station this is relevant for
        /// e.g. base sauces and pizza toppings are "makeline"
        /// e.g. post bakes and sides sauces are "cutbench"
        /// </summary>
        [XmlElement]
        public string OptionTypeCode { get; set; }

        /// <summary>
        /// No idea what this is for. Seems to always be 87
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public int PizzaDistribution { get; set; }

        /// <summary>
        /// Not sure what this is for. Seems to be some internal enum value
        /// code 45 = qty 0
        /// code 49 = qty 1
        /// Sometimes this is empty. eg added chilli flakes, added bbq sauce, added vegan cheese
        /// </summary>
        [XmlElement]
        public int? ToppingAmountCode { get; set; }

        /// <summary>
        /// Full text version of `ToppingAmountCode`
        /// </summary>
        [XmlElement]
        public string ToppingAmountDescription { get; set; }

        /// <summary>
        /// Topping code that was added or removed
        /// </summary>
        [XmlElement]
        public string ToppingCode { get; set; }

        /// <summary>
        /// Full text version of `ToppingCode`
        /// </summary>
        [XmlElement]
        public string ToppingDescription { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MakeLineToppingModification);
        }

        public bool Equals(MakeLineToppingModification? other)
        {
            return other is not null &&
                   DisplaySequence == other.DisplaySequence &&
                   IsHvi == other.IsHvi &&
                   OptionTypeCode == other.OptionTypeCode &&
                   ToppingAmountCode == other.ToppingAmountCode &&
                   ToppingCode == other.ToppingCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DisplaySequence, IsHvi, OptionTypeCode, ToppingAmountCode, ToppingCode);
        }

        public static bool operator ==(MakeLineToppingModification? left, MakeLineToppingModification? right)
        {
            return EqualityComparer<MakeLineToppingModification>.Default.Equals(left, right);
        }

        public static bool operator !=(MakeLineToppingModification? left, MakeLineToppingModification? right)
        {
            return !(left == right);
        }
    }
}
