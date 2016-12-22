using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Models;
using ECT.Model.Abstract;
using ECT.Model.Entities;

namespace ECT.FormLib.Areas.Netherlands.Controllers
{
    public class AdditionalPaymentsDeductionController : FormLib.Controllers.FormLibBaseController
    {
        public AdditionalPaymentsDeductionController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

