using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;

namespace DH.Helpdesk.EForm.FormLib.Areas.SouthKorea.Controllers
{
    public class PersonalInformationController : FormLibBaseController
    {
        public PersonalInformationController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}
