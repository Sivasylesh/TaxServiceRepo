using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TaxService.Queries;

namespace TaxService.QueryHandlers
{
    public class GetTaxDetailsQueryHandler : IRequestHandler<GetTaxDetailsQuery, List<string>>
    {
        private readonly ILogger<GetTaxDetailsQueryHandler> _logger;
        private decimal _result = 0.0M;
        public GetTaxDetailsQueryHandler(ILogger<GetTaxDetailsQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> Handle(GetTaxDetailsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                //Business Logic            
                var date = DateTime.Parse(query.Date);
                switch (query.Municipality)
                {
                    case "Vilnius":
                        CheckVilniusDailyTaxes(date);
                        if (_result == 0.0M)
                        {
                            CheckVilniusMonthlyTaxes(date);
                        }
                        if (_result == 0.0M)
                        {
                            CheckVilniusYearlyTaxes(date);
                        }
                        break;

                    case "Kaunas":
                        break;
                    default:
                        break;
                }
                return new List<string> { _result.ToString() };
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error occurred in Handler. Error Message: {ex.Message}");
                return new List<string> { _result.ToString() };
            }
        }

        private decimal CheckVilniusDailyTaxes(DateTime date)
        {
            if (date.Equals(DateTime.Parse("2020.01.01")) || date.Equals(DateTime.Parse("2020.12.25")))
            {
                _result = 0.1M;
            }
            return _result;
        }

        private decimal CheckVilniusMonthlyTaxes(DateTime date)
        {
            if (date>=(DateTime.Parse("2020.05.01")) && date<=(DateTime.Parse("2020.05.31")))
            {
                _result = 0.4M;
            }
            return _result;
        }

        private decimal CheckVilniusYearlyTaxes(DateTime date)
        {
            if (date >= (DateTime.Parse("2020.01.01")) && date <= (DateTime.Parse("2020.12.31")))
            {
                _result = 0.2M;
            }
            return _result;
        }
    }
}
