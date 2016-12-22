using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Global.Controllers
{
    public class PersonalInfoChangeController : FormLib.Controllers.FormLibBaseController
    {
        public PersonalInfoChangeController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}
