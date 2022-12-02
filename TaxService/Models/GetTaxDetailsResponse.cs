namespace TaxService.Models
{
    public class GetTaxDetailsResponse
    {
        public string Municipality { get; set; }

        public string RequestedDate { get; set; }

        public int TaxRule { get; set; }

        public decimal Result { get; set; }

        public string Errors { get; set; }
    }
}
