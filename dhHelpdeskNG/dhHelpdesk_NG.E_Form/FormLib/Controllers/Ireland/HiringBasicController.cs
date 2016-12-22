using System;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Ireland.Controllers
{
    public class HiringBasicController : FormLibBaseController
    {
        public HiringBasicController(IContractRepository contractRepository, IFileService fileService, IUserRepository userRepository)
            : base(userRepository, contractRepository, fileService)
        {
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
