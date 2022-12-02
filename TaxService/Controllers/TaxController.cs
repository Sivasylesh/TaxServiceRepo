using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        [HttpGet,Route("getdetails")]
        public async Task<IActionResult> GetTaxDetails([FromQuery] string municipality, [FromQuery] string date)
        {
            _logger.LogInformation($"Entered GetTaxDetails Api");
            var query = new GetTaxDetailsQuery(municipality,date);
            var result = await _mediator.Send(query);
            _logger.LogInformation($"Finished GetTaxDetails Api with Response: {JsonConvert.SerializeObject(result)}");
            return Ok(result);
        }
    }
}
