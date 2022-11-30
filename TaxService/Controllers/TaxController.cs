using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxService.Controllers
{
    [Route("api/tax")]
    public class TaxController : ControllerBase
    {
        [HttpGet,Route("get")]
        public IActionResult GetTaxDetails()
        {
            return Ok();
        }
    }
}
