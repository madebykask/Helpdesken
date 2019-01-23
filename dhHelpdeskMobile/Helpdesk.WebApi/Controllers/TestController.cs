using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("{id}")]
        //[Route("TestIt")]
        public async Task<IActionResult> TestIt(long id)
        {
            return await Task.FromResult(Ok(id));
        }
    }
}