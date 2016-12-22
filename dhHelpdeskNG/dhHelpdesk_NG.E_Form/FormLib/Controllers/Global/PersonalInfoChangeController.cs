using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Global.Controllers
{
    public class PersonalInfoChangeController : FormLib.Controllers.FormLibBaseController
    {
        public PersonalInfoChangeController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}
