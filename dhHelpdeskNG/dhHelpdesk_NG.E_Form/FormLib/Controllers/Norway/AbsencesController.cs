using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Models;
using ECT.FormLib.Pdfs;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class AbsencesController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public AbsencesController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    
    }  
}

