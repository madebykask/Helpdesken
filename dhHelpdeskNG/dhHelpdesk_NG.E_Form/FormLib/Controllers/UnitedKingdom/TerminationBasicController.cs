using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.FormLib.Models;
using ECT.FormLib.Pdfs;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace ECT.FormLib.Areas.UnitedKingdom.Controllers
{
    public class TerminationBasicController : FormLibBaseController
    {

        public const string xmlPath = "unitedkingdom/terminationbasic.xml";
        private readonly IContractRepository _contractRepository;

        public TerminationBasicController(IContractRepository contractRepository, IFileService fileService, IUserRepository userRepository)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
        }

       
    }
}