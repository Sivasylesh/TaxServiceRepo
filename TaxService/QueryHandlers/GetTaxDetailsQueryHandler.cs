using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TaxService.Database;
using TaxService.Models;
using TaxService.Queries;

namespace TaxService.QueryHandlers
{
    public class GetTaxDetailsQueryHandler : IRequestHandler<GetTaxDetailsQuery, GetTaxDetailsResponse>
    {
        private readonly ILogger<GetTaxDetailsQueryHandler> _logger;
        private readonly IDataBaseRepository _dataBaseRepository;
        private decimal _result = 0.0M;

        public GetTaxDetailsQueryHandler(ILogger<GetTaxDetailsQueryHandler> logger, IDataBaseRepository dataBaseRepository)
        {
            _logger = logger;
            _dataBaseRepository = dataBaseRepository;
        }

        public async Task<GetTaxDetailsResponse> Handle(GetTaxDetailsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Entered Handler for Fetching Tax Details");
            var result = new GetTaxDetailsResponse();
            try
            {
                //Business Logic
                if (string.IsNullOrWhiteSpace(query.Date))
                {
                    throw new Exception("Input Date cannot be Empty");
                }
                DateTime.TryParse(query.Date, out var date);

                //Fetch Tax Details from Database
                var taxDetails = await _dataBaseRepository.FetchTaxDetailsFromDB();

                switch (query.Municipality.ToUpper())
                {
                    case "VILNIUS":                        
                        CheckVilniusDailyTaxes(date, taxDetails);
                        if (_result == 0.0M)
                        {
                            CheckVilniusMonthlyTaxes(date, taxDetails);
                        }
                        if (_result == 0.0M)
                        {
                            CheckVilniusYearlyTaxes(date, taxDetails);
                        }

                        result.Municipality = "Vilnius";
                        result.RequestedDate = query.Date;
                        result.TaxRule = 2;
                        result.Result = _result;
                        break;

                    case "KAUNAS":
                        CheckKaunasWeeklyTaxes(date, taxDetails);
                        CheckKaunasMonthlyTaxes(date, taxDetails);
                        CheckKaunasYearlyTaxes(date, taxDetails);

                        result.Municipality = "Kaunas";
                        result.RequestedDate = query.Date;
                        result.TaxRule = 1;
                        result.Result = _result;
                        break;

                    default:
                        break;
                }
                
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error occurred in Handler. Error Message: {ex.Message}");
                result.Errors = ex.Message;
            }
            _logger.LogInformation($"Finished Handler for Fetching Tax Details");
            return result;
        }

        #region Vilnius Tax Calculation Methods

        private decimal CheckVilniusDailyTaxes(DateTime date, List<TaxDetails> taxDetails)
        {
            var individualDates = taxDetails.Where(x => x.Municipality?.ToUpper() == "VILNIUS" && x.TaxType == "Daily").Select(y => y.IndividualDates)?.FirstOrDefault();
            var datesList = individualDates?.Split(',')?.ToList() ?? new List<string>();
            foreach (var taxdate in datesList)
            {
                if (date.Equals(DateTime.Parse(taxdate)))
                {
                    _result = 0.1M;
                }
            }
            return _result;
        }

        private decimal CheckVilniusMonthlyTaxes(DateTime date, List<TaxDetails> taxDetails)
        {
            var fromDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "VILNIUS" && x.TaxType == "Monthly").Select(y => y.FromDate)?.FirstOrDefault();
            var toDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "VILNIUS" && x.TaxType == "Monthly").Select(y => y.ToDate)?.FirstOrDefault();
            if (date>=fromDate && date<=toDate)
            {
                _result = 0.4M;
            }
            return _result;
        }

        private decimal CheckVilniusYearlyTaxes(DateTime date, List<TaxDetails> taxDetails)
        {
            var fromDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "VILNIUS" && x.TaxType == "Yearly").Select(y => y.FromDate)?.FirstOrDefault();
            var toDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "VILNIUS" && x.TaxType == "Yearly").Select(y => y.ToDate)?.FirstOrDefault();
            if (date >= fromDate && date <= toDate)
            {
                _result = 0.2M;
            }
            return _result;
        }

        #endregion Vilnius Tax Calculation Methods

        #region Kaunas Tax Calculation Methods

        private decimal CheckKaunasWeeklyTaxes(DateTime date, List<TaxDetails> taxDetails)
        {
            var fromDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "KAUNAS" && x.TaxType == "Weekly").Select(y => y.FromDate)?.FirstOrDefault();
            var toDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "KAUNAS" && x.TaxType == "Weekly").Select(y => y.ToDate)?.FirstOrDefault();
            if (date >= fromDate && date <= toDate)
            {
                _result += 0.1M;
            }
            return _result;
        }

        private decimal CheckKaunasMonthlyTaxes(DateTime date, List<TaxDetails> taxDetails)
        {
            var fromDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "KAUNAS" && x.TaxType == "Monthly").Select(y => y.FromDate)?.FirstOrDefault();
            var toDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "KAUNAS" && x.TaxType == "Monthly").Select(y => y.ToDate)?.FirstOrDefault();
            if (date >= fromDate && date <= toDate)
            {
                _result += 0.2M;
            }
            return _result;
        }

        private decimal CheckKaunasYearlyTaxes(DateTime date, List<TaxDetails> taxDetails)
        {
            var fromDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "KAUNAS" && x.TaxType == "Yearly").Select(y => y.FromDate)?.FirstOrDefault();
            var toDate = taxDetails.Where(x => x.Municipality?.ToUpper() == "KAUNAS" && x.TaxType == "Yearly").Select(y => y.ToDate)?.FirstOrDefault();
            if (date >= fromDate && date <= toDate)
            {
                _result += 0.3M;
            }
            return _result;
        }

        #endregion Kaunas Tax Calculation Methods
    }
}
