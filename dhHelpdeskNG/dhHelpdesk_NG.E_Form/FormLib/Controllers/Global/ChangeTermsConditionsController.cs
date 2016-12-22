using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Global.Controllers
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

