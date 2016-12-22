using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.FormLib.Pdfs;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers
{
    public class TerminationCompleteController : DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {

        public TerminationCompleteController(IContractRepository contractRepository
             , IUserRepository userRepository
            , IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }

    }
}