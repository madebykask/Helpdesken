using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.FormLib.Models;
using ECT.FormLib.Pdfs;
using ECT.Model.Abstract;
using ECT.Model.Entities;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class NorwayBaseController : FormLibBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public NorwayBaseController() : base() { }

        public NorwayBaseController(IUserRepository userRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public NorwayBaseController(IUserRepository userRepository, IContractRepository contractRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _fileService = fileService;
        }

        /// <summary>Narrowing down CostCentres by Department. Used in Norway
        /// Based on that Narrowing XML is placed in XMLS/Narrowing/
        /// Named: ND_Department_CostCentre.xml
        /// If no match, all is returned. Based on that XML is placed in /XMLS/Data
        /// Named: Data_Global_CostCentre.xml
        /// </summary>
        public JsonResult GetCostCentresByDepartment(int departmentId)
        {
            //TODO: Move logic from here /TAN
            //TODO: XML path, better
            //TODO: Path to XML:s, can we set this somewhere else?
            string baseValue = "";

            string area = HttpContext.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            string predefinedPath = area + "/Narrowing/ND_Department_CostCentre.xml";

            if (departmentId > 0)
            {
                //Get name from DB
                var department = _contractRepository.GetOuById(departmentId);
                baseValue = department.Name;
            }

            var di = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory));
            var path = di.FullName;

            path = Path.Combine(di.FullName, predefinedPath);

            var xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(path);

            var xDoc = xmlDocument.ToXDocument();

            var options = xDoc.Descendants("option").Where(x => x.Parent.Attribute("baseValue") != null
                && x.Parent.Attribute("baseValue").Value.ToLower() == baseValue.ToLower()).Select(x => new Option { Name = x.Value, Value = x.Attribute("value").Value, Id = (x.Attribute("value").Value == "" ? 0 : int.Parse(x.Attribute("value").Value)) }).ToArray();

            //If empty - get all
            if (options.Count() == 0)
            {
                path = Path.Combine(di.FullName, area + "/data/Data_Global_CostCentre.xml");
                var xmlDocumentAll = new System.Xml.XmlDocument();
                xmlDocumentAll.Load(path);
                var xDoc2 = xmlDocumentAll.ToXDocument();

                options = xDoc2.Descendants("option").Select(x => new Option { Name = x.Value, Value = x.Attribute("value").Value, Id = (x.Attribute("value").Value == "" ? 0 : int.Parse(x.Attribute("value").Value)) }).ToArray();
            }

            return Json(options, JsonRequestBehavior.AllowGet);
        }

    }
}
