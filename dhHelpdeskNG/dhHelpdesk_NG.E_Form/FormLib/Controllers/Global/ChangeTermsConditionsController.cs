using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Global.Controllers
{
    public class ChangeTermsConditionsController : FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;
        private readonly IFileService _fileService;

        public ChangeTermsConditionsController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
            _fileService = fileService;
        }
    }
}

