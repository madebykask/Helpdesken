using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers
{
    public class NewHireNotShowingUpController : DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public NewHireNotShowingUpController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

