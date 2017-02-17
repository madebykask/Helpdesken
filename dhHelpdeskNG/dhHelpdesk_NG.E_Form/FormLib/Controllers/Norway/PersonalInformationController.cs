using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using DH.Helpdesk.EForm.FormLib.Controllers;

namespace DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers
{
    public class PersonalInformationController : DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public PersonalInformationController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

