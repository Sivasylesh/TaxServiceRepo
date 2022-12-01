using MediatR;
using System.Collections.Generic;

namespace TaxService.Queries
{
    public class GetTaxDetailsQuery : IRequest<List<string>>
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
