using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Global.Controllers
{
    public class AdditionalPaymentsDeductionController : FormLib.Controllers.FormLibBaseController
    {
        public AdditionalPaymentsDeductionController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

