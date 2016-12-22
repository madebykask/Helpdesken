using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.UnitedKingdom.Controllers
{
    public class VoluntaryDeductionsController : FormLib.Controllers.FormLibBaseController
    {
        public VoluntaryDeductionsController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

