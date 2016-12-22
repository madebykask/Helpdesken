using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Models;
using ECT.Model.Abstract;
using ECT.Model.Entities;

namespace ECT.FormLib.Areas.Norway.Controllers
{
    public class AdditionalPaymentsDeductionController : ECT.FormLib.Areas.Norway.Controllers.NorwayBaseController
    {
        public AdditionalPaymentsDeductionController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
        }
    }
}

