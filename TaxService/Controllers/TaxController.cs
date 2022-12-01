using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TaxService.Queries;

namespace TaxService.Controllers
{
    [Route("api/tax")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TaxController> _logger;

        public TaxController(IMediator mediator, ILogger<TaxController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet,Route("get")]
        public async Task<IActionResult> GetTaxDetails(string municipality, string date)
        {
            _logger.LogInformation("Entered GetTaxDetails Api");
            var query = new GetTaxDetailsQuery(municipality,date);
            var result = await _mediator.Send(query);
            _logger.LogInformation("Finished GetTaxDetails Api");
            return Ok(result);
        }
    }
}
