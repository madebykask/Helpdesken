using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers
{
    public class IncorrectWorkedHoursController : DH.Helpdesk.EForm.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public IncorrectWorkedHoursController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

