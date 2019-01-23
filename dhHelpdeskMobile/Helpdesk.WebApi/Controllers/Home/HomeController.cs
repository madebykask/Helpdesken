using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers.Home
{
    /// <summary>
    /// Created for test purposes - identityserver4 redirects to this url by defuylat - will be removed soon
    /// </summary>
    [AllowAnonymous]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _identity;

        public HomeController(IIdentityServerInteractionService identity)
        {
            _identity = identity;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Route("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var errormessage = await _identity.GetErrorContextAsync(errorId);
            return await Task.FromResult(new JsonResult(errormessage));
        }
    }
}