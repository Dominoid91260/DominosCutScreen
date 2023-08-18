using System.Xml.Serialization;

namespace DominosCutScreen.Shared
{
    [XmlRoot("ArrayOfMakeLineOrderContract")]
    public class ArrayOfMakeLineOrder
    {
        /// <summary>
        /// List of all orders currently on the makeline
        /// </summary>
        [XmlElement("MakeLineOrderContract")]
        public List<MakeLineOrder> Orders { get; set; }
    }
}
