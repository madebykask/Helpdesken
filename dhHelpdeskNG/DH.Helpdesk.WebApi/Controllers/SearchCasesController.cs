using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;

namespace DH.Helpdesk.WebApi.Controllers
{
    [AuthorizeApi]
    public class SearchCasesController : BaseApiController
    {
    }
}