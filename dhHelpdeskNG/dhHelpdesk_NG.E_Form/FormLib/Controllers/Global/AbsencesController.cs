using System.Web.Mvc;
using ECT.Core.Service;
using ECT.Model.Abstract;

namespace ECT.FormLib.Areas.Global.Controllers
{
    public class AbsencesController : FormLib.Controllers.FormLibBaseController
    {
        public AbsencesController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }      
    }
}

