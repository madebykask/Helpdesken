using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using DH.Helpdesk.EForm.FormLib.Controllers;

namespace DH.Helpdesk.EForm.FormLib.Areas.Netherlands.Controllers
{
    public class PersonalInformationController : FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;

        public PersonalInformationController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

