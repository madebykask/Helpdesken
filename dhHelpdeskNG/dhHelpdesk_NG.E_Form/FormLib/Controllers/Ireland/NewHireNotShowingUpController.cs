using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Ireland.Controllers
{
    public class NewHireNotShowingUpController : FormLib.Controllers.FormLibBaseController
    {
        public NewHireNotShowingUpController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

