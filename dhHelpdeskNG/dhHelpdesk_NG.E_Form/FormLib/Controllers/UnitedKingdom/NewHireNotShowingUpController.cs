using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.UnitedKingdom.Controllers
{
    public class NewHireNotShowingUpController : FormLib.Controllers.FormLibBaseController
    {
        public NewHireNotShowingUpController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

