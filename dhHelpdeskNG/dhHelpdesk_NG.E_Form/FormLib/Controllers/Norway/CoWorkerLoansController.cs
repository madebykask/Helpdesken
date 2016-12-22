using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class CoWorkerLoansController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public CoWorkerLoansController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

