using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominosCutScreen.Shared
{
    public static class MakelineOverrideManager
    {

        // This sets a crust/base from a key found in the name of the pizza
        // These can't contain spaces since we remove them. See MakeLineOrderLine.OnDeserializedMethod.
        public static readonly Dictionary<string, string> CrustNameOverrides = new()
        {
            { "MDB", "MDB" },
            { "Mini", "Mini" },
            { "Deep", "Deep" },
            { "CLASS", "Classic" },
            { "XL", "XL" },
            { "Thin", "Thin" },
            { "Crust", "Cheesy Crust" },
            { "GLUTEN", "Gluten Free" },
            { "Free", string.Empty } // This is only here because it appears as `GLUTEN FREE` and when we remove the spaces, `GLUTENFREE` no longer matches the description so it fails to get removed.
        };

        public static readonly Dictionary<string, string> ProductNameOverrides = new() {
        };
    }
}
