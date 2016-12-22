using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Netherlands.Controllers
{
    public class VoluntaryDeductionsController : FormLibBaseController
    {
        public VoluntaryDeductionsController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

