using MediatR;
using System.Collections.Generic;
using TaxService.Models;

namespace TaxService.Queries
{
    public class GetTaxDetailsQuery : IRequest<GetTaxDetailsResponse>
    {
        public string Municipality { get; set; }
        public string Date { get; set; }

        public GetTaxDetailsQuery(string municipality, string date)
        {
            Municipality = municipality;
            Date = date;
        }
    }
}
