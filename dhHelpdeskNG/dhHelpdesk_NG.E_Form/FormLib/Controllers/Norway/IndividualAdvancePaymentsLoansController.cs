using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class IndividualAdvancePaymentsLoansController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public IndividualAdvancePaymentsLoansController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}