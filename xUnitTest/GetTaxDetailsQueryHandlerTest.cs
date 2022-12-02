using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using TaxService.Database;
using TaxService.Models;
using TaxService.Queries;
using TaxService.QueryHandlers;
using Xunit;

namespace xUnitTest
{
    public class GetTaxDetailsQueryHandlerTest: IDisposable  
    {        

        private Mock<IDataBaseRepository> _executorFactory;
        private GetTaxDetailsQueryHandler _mockHandler;
        private GetTaxDetailsQuery _mockQuery;
        private GetTaxDetailsResponse _exeResult;        

        /// <summary>
        /// To test handler without any exceptions from DB
        /// </summary>
        [Fact(DisplayName = "GetTaxDetails QueryHandler: Positive Scenario which returns the TaxDetails Response")]
        public void HandlerPositiveTest()
        {
            SetUpDBWithoutExceptions();
            CheckSuccessResult();
        }

        /// <summary>
        /// To test handler when generic exception occurs
        /// </summary>
        ///
        [Fact(DisplayName = "GetTaxDetails QueryHandler: Negative Scenario: Generic exception")]
        public void HandlerWithExceptionNegativeTest()
        {
            SetUpDBGenericException();
            CheckDBExceptionResult();
        }

        #region Private Methods

        /// <summary>
        /// Mock DB service without exceptions
        /// </summary>
        private void SetUpDBWithoutExceptions()
        {
            //create mock factory 
            _executorFactory = new Mock<IDataBaseRepository>(MockBehavior.Loose);
            var logger = new Mock<ILogger<GetTaxDetailsQueryHandler>>(MockBehavior.Loose);

            //set up partnerDetails to return Partner Config details
            var taxDetails = new List<TaxDetails> {
                new TaxDetails {
                    Municipality="Vilnius",
                    TaxType ="Yearly",
                    TaxRule =2,
                    FromDate =DateTime.Parse("2020-01-01"),
                    ToDate =DateTime.Parse("2020-12-31"),
                    TaxApplied = 0.2M
               }
            };

            //Mocking the procedure execution  
            _executorFactory.Setup(m => m.FetchTaxDetailsFromDB().Result).Returns(taxDetails);            

            //Mocking the call using dependency injection
            _mockHandler = new GetTaxDetailsQueryHandler(logger.Object, _executorFactory.Object);
            _mockQuery = new GetTaxDetailsQuery("Vilnius", "2020-01-01");
            _exeResult = _mockHandler.Handle(_mockQuery, new CancellationToken()).Result;
        }

        /// <summary>
        /// Mock DB service with generic exceptions
        /// </summary>     
        private void SetUpDBGenericException()
        {
            //Mocking the connection
            _executorFactory = new Mock<IDataBaseRepository>(MockBehavior.Loose);
            var logger = new Mock<ILogger<GetTaxDetailsQueryHandler>>(MockBehavior.Loose);

            _executorFactory.Setup(m => m.FetchTaxDetailsFromDB()).Throws(new System.Exception());

            //Mocking the procedure execution
            //_executorFactory.Setup(m => m.GetDbExecutor().Query<PartnerDetails>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CommandType>())).Throws(new System.Exception());
            _mockHandler = new GetTaxDetailsQueryHandler(logger.Object, _executorFactory.Object);
            _mockQuery = new GetTaxDetailsQuery("Vilnius", "2020-01-01");
            _exeResult = _mockHandler.Handle(_mockQuery, new CancellationToken()).Result;
        }

        /// <summary>
        /// To check the expected Tax details
        /// </summary>
        private void CheckSuccessResult()
        {
            Xunit.Assert.NotNull(_exeResult);
        }

        /// <summary>
        /// To check the DB Exception result
        /// </summary>
        private void CheckDBExceptionResult()
        {
            Xunit.Assert.NotNull(_exeResult.Errors);
        }
        #endregion 

        #region Tear Down

        /// <summary>
        /// to dispose data setup/mock objects
        /// </summary>
        public void Dispose()
        {
            _executorFactory = null;
            _mockHandler = null;
            _mockQuery = null;
            _exeResult = null;
        }

        #endregion
    }
}
