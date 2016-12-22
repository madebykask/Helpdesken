using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class NewHireNotShowingUpController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public NewHireNotShowingUpController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

