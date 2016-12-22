using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Service;

namespace FormLibDemo.Controllers
{
    public class FormHandlerController : Controller
    {
        private readonly IFormService _service;

        public FormHandlerController(IFormService service)
        {
            _service = service;
        }
    }
}
