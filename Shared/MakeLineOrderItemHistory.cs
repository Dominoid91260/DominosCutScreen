using System.Xml.Serialization;

namespace DominosCutScreen.Shared
{
    public class MakeLineOrderItemHistory : CommonMakelineItem, IEquatable<MakeLineOrderItemHistory?>
    {
        /// <summary>
        /// No idea what this is for. Seems to always be 0
        /// </summary>
        [XmlElement]
        public int LinePart { get; set; }

        /// <summary>
        /// Pulse order number
        /// </summary>
        [XmlElement("Number")]
        public int OrderNumber { get; set; }

        /// <summary>
        /// Not sure what this is for. I'd guess its if the order was edited after items were bumped
        /// </summary>
        [XmlElement]
        public DateTime OrderEditDate { get; set; }

        /// <summary>
        /// Carry-out or Delivery (C or D)
        /// </summary>
        [XmlElement]
        public string TypeCode { get; set; }

        /// <summary>
        /// Full text version of <see cref="TypeCode"/>
        /// </summary>
        [XmlElement]
        public string ServiceMethodGroup { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MakeLineOrderItemHistory);
        }

        public bool Equals(MakeLineOrderItemHistory? other)
        {
            return other is not null &&
                   Description == other.Description &&
                   OrderNumber == other.OrderNumber &&
                   EqualityComparer<List<MakeLineToppingModification>>.Default.Equals(ToppingModifications, other.ToppingModifications);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description, OrderNumber, ToppingModifications);
        }

        public static bool operator ==(MakeLineOrderItemHistory? left, MakeLineOrderItemHistory? right)
        {
            return EqualityComparer<MakeLineOrderItemHistory>.Default.Equals(left, right);
        }

        public static bool operator !=(MakeLineOrderItemHistory? left, MakeLineOrderItemHistory? right)
        {
            return !(left == right);
        }

        public void OnDeserializedMethod()
        {
            OnDeserialized();
        }
    }
}
