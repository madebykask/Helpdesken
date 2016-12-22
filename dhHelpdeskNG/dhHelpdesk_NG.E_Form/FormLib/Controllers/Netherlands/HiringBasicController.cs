using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.FormLib.Pdfs;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using System.Linq;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace DH.Helpdesk.EForm.FormLib.Areas.Netherlands.Controllers
{
    public class HiringBasicController : FormLibBaseController
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
