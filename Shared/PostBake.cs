using System.ComponentModel.DataAnnotations;

namespace DominosCutScreen.Shared
{
    public class PostBake
    {
        [Key]
        public string ReceiptCode { get; set; }

        public string ToppingCode { get; set; }
        public string ToppingDescription { get; set; }
        public bool IsEnabled { get; set; }

        public MakeLineToppingModification AsToppingModification() => new()
        {
            OptionTypeCode = "cutbench",
            ToppingCode = ToppingCode,
            ToppingDescription = $"*** {ToppingDescription}",
            //ToppingDistribution is set in `MakelineItemTransformer.ParsePostBakes`
            ToppingAmountCode = 49
        };
    }
}
