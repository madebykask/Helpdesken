using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class IncorrectWorkedHoursController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public IncorrectWorkedHoursController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

