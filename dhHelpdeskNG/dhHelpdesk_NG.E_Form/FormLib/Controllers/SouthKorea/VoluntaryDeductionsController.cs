using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.SouthKorea.Controllers
{
    public class VoluntaryDeductionsController : FormLib.Controllers.FormLibBaseController
    {
        public VoluntaryDeductionsController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

