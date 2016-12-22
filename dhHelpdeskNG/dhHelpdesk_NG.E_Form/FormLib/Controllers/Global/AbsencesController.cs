using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.Global.Controllers
{
    public class AbsencesController : FormLib.Controllers.FormLibBaseController
    {
        public AbsencesController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }      
    }
}

