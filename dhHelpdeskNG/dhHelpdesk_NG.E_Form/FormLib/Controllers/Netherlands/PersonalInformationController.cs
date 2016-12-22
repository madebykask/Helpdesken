using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Models;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using ECT.FormLib.Controllers;

namespace ECT.FormLib.Areas.Netherlands.Controllers
{
    public class PersonalInformationController : FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;

        public PersonalInformationController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }

        public JsonResult Typeahead(string query, string node, string dependentAttribute, string dependentAttributeValue)
        {
            var model = new FormModel(mainXmlPath);
            var element = model.GetElement(node);

            if (element == null && element.Descendants("option").Any())
                return Json(new object { });

            var options = element.Descendants("option").Where(x => x.Attribute(dependentAttribute) != null
                && x.Attribute(dependentAttribute).Value == dependentAttributeValue
                || string.IsNullOrEmpty(dependentAttributeValue)).Select(x => x.Value).ToArray();

            return Json(new { options });
        }
    }
}

