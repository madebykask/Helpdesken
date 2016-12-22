using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Service;

namespace FormLibDemo.Controllers
{
    public class FormEditorController : Controller
    {
        private readonly IFormService _service;

        public FormEditorController(IFormService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var forms = _service.Forms(Guid.NewGuid());
            return View(forms);
        }

        public ActionResult Edit(int id)
        {
            var form = _service.InitForm(Guid.NewGuid());
            return View(form);
        }
	}
}