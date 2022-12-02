using System;

namespace TaxService.Models
{
    public class TaxDetails
    {
        public int TaxID { get; set; }

        public string Municipality { get; set; }

        public string TaxType { get; set; }

        public int TaxRule { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string IndividualDates { get; set; }

        public decimal TaxApplied { get; set; }
    }
}
