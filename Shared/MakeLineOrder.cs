using System.Xml.Serialization;

namespace DominosCutScreen.Shared
{
    public class MakeLineOrder : IEquatable<MakeLineOrder?>
    {
        /// <summary>
        /// When this order was actually placed.
        /// For timed orders, this is the time wanted.
        /// </summary>
        [XmlElement]
        public DateTime ActualOrderedAt { get; set; }

        /// <summary>
        /// Delivery address for this order
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Not sure what this is for. I'd guess its for sorting orders on the makeline
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public int DisplayIndex { get; set; }

        /// <summary>
        /// Not sure what this is for. I'd guess its to say when an order was expedited to the makeline
        /// </summary>
        [XmlElement]
        public DateTime ExpeditedAt { get; set; }

        /// <summary>
        /// Whether all items for this order have been bumped or not
        /// </summary>
        [XmlElement]
        public bool IsBumped { get; set; }

        /// <summary>
        /// Whether or not this order is currently open in an order taker
        /// </summary>
        [XmlElement]
        public bool IsBeingModified { get; set; }

        /// <summary>
        /// Does this order require a callback?
        /// Not sure what the conditions are for this
        /// </summary>
        [XmlElement]
        public bool IsCallbackRequired { get; set; }

        /// <summary>
        /// Was the order placed online?
        /// </summary>
        [XmlElement]
        public bool IsElectronic { get; set; }

        // OrderSourceType

        /// <summary>
        /// Has the order been expedited?
        /// </summary>
        [XmlElement]
        public bool IsExpedited { get; set; }

        /// <summary>
        /// No idea what this is for. Seems to always be false
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public bool IsLda { get; set; }

        /// <summary>
        /// No idea what this is for. Seems to always be false
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public bool IsLoyaltyMember { get; set; }

        /// <summary>
        /// Whether or not this phone number existed in pulse before this order
        /// </summary>
        [XmlElement]
        public bool IsNewCustomer { get; set; }

        /// <summary>
        /// Whether or not a previous order for today exists with this phone number
        /// </summary>
        [XmlElement]
        public bool IsPotentialDuplicate { get; set; }

        /// <summary>
        /// No idea what this is for or how it differs from `IsExpedited`
        /// </summary>
        [XmlElement]
        public bool IsPriorityOrder { get; set; }

        /// <summary>
        /// No idea what this is for.
        /// </summary>
        [XmlElement]
        public bool IsProductRemake { get; set; }

        /// <summary>
        /// Not sure what this is for. I'd guess its true if all the items in the order are also remakes
        /// </summary>
        /// @TODO confirm
        [XmlElement]
        public bool IsRemake { get; set; }

        /// <summary>
        /// Whether or not this order is timed for later today
        /// </summary>
        [XmlElement]
        public bool IsTimedOrder { get; set; }

        /// <summary>
        /// List of items in this order
        /// </summary>
        [XmlArray, XmlArrayItem("MakeLineOrderLineContract")]
        public List<MakeLineOrderLine> Items { get; set; }

        /// <summary>
        /// When the order was last modified
        /// If the order was never modified this seems to be almost the same as `ActualOrderedAt` within a few seconds.
        /// For timed orders, this seems to be the time it appeared in pulse.
        /// </summary>
        [XmlElement]
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Pulse order number
        /// </summary>
        [XmlElement("Number")]
        public int OrderNumber { get; set; }

        // OneTimeCustomerComments

        /// <summary>
        /// When the order was actually sent to pulse.
        /// For timed orders, this is the time it will appear on the makeline
        /// </summary>
        [XmlElement]
        public DateTime SavedAt { get; set; }

        /// <summary>
        /// CARRYOUT or DELIVERY
        /// </summary>
        [XmlElement]
        public string ServiceMethodGroup { get; set; }

        /// <summary>
        /// No idea what this is for. Seems to always be 1
        /// </summary>
        [XmlElement]
        public int StatusCode { get; set; }

        /// <summary>
        /// CSR that took the order or empty for internet orders
        /// </summary>
        [XmlElement]
        public string TakenBy { get; set; }

        /// <summary>
        /// Carry-out or Delivery code (C or D)
        /// </summary>
        [XmlElement]
        public string TypeCode { get; set; }

        /// <summary>
        /// Full text version of <see cref="TypeCode"/>
        /// </summary>
        [XmlElement]
        public string TypeCodeDescription { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MakeLineOrder);
        }

        public bool Equals(MakeLineOrder? other)
        {
            return other is not null &&
                   OrderNumber == other.OrderNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderNumber);
        }

        public static bool operator ==(MakeLineOrder? left, MakeLineOrder? right)
        {
            return EqualityComparer<MakeLineOrder>.Default.Equals(left, right);
        }

        public static bool operator !=(MakeLineOrder? left, MakeLineOrder? right)
        {
            return !(left == right);
        }

        // OrderUnhiddenTime

        public bool IsLive()
        {
            if (IsBumped)
                return false;

            if (IsTimedOrder)
                return SavedAt >= DateTime.Now;

            return ActualOrderedAt >= DateTime.Now;
        }
    }
}
