using System.Xml.Serialization;

namespace DominosCutScreen.Shared
{
    [XmlRoot("ArrayOfMakeLineOrderItemHistoryContract")]
    public class ArrayOfMakeLineOrderItemHistory
    {
        /// <summary>
        /// List of all items that have been bumped.
        /// Pretty sure this is limited either by time or count
        /// </summary>
        [XmlElement("MakeLineOrderItemHistoryContract")]
        public List<MakeLineOrderItemHistory> Items { get; set; }
    }
}
