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

    }
}
