using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.FormLib.Areas.Netherlands.Controllers
{
    public class AdditionalPaymentsDeductionController : FormLib.Controllers.FormLibBaseController
    {
        public AdditionalPaymentsDeductionController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

