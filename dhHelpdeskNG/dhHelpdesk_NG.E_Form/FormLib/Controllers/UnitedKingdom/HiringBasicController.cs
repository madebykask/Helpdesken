using System;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.UnitedKingdom.Controllers
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
