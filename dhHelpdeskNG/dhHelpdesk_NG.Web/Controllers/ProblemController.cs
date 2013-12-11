using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class ProblemController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IProblemService _problemService;
        private readonly IUserService _userService;

        public ProblemController(
            ICustomerService customerService,
            IProblemService problemService,
            IUserService userService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _customerService = customerService;
            _problemService = problemService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var problem = _problemService.GetProblems(SessionFacade.CurrentCustomer.Id);

            return View(problem);
        }

        private ProblemInputViewModel CreateInputViewModel(Problem problem)
        {
            var model = new ProblemInputViewModel
            {
                Problem = problem,
                Customers = _customerService.GetCustomers(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Users = _userService.GetUsers().Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.SurName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
