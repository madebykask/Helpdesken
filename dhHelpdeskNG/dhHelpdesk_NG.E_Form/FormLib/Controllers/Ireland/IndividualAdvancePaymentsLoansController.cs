using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Ireland.Controllers
{
    public class IndividualAdvancePaymentsLoansController : FormLib.Controllers.FormLibBaseController
    {
        public IndividualAdvancePaymentsLoansController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

