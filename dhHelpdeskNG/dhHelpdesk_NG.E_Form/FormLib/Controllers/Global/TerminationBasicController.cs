using System;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Global.Controllers
{
    public class TerminationBasicController : FormLibBaseController
    {
        public TerminationBasicController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
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