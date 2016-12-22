using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.FormLib.Models;
using ECT.FormLib.Pdfs;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using System.Linq;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class HiringBasicController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        private readonly IContractRepository _contractRepository; 
       
        public HiringBasicController(IContractRepository contractRepository, IFileService fileService, IUserRepository userRepository)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
        }

        public override ActionResult New()
        {
            throw new NotImplementedException();
        }

        public override ActionResult New(FormCollection formCollection, string[] uploads)
        {
            throw new NotImplementedException();
        }
    }
}
