using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominosCutScreen.Shared
{
    public static class MakelineOverrideManager
    {
        public static readonly Dictionary<string, MakeLineToppingModification> PostBakeOverrides = new()
        {
            { "*", new() { ToppingCode = "spo", ToppingDescription = "Spring Onion" } },
            { "B", new() { ToppingCode = "BUTT", ToppingDescription = "Butter Chicken Sce" } }, // Confirm
            { "Hb", new() { ToppingCode = "HICBBQ", ToppingDescription = "Hickory BBQ" } },
            { "Ho", new() { ToppingCode = "HOLLAND", ToppingDescription = "Hollandaise" } },
            { "M", new() { ToppingCode = "My", ToppingDescription = "Mayonnaise" } },
            { "P", new() { ToppingCode = "PERI", ToppingDescription = "Peri Peri Sce" } },
            { "T", new() { ToppingCode = "TOMCAP", ToppingDescription = "Tom Caps Sce" } } // Confirm
        };

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
