namespace dhHelpdesk_NG.Web.Controllers
{
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Models.Problem.Input;
    using dhHelpdesk_NG.Web.Models.Problem.Output;

    public class ProblemsController : BaseController
    {
        private readonly ICustomerService customerService;
        private readonly IProblemService problemService;
        private readonly IUserService userService;

        public ProblemsController(
                ICustomerService customerService,
                IProblemService problemService,
                IMasterDataService masterDataService,
                IUserService userService)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.problemService = problemService;
            this.userService = userService;
        }

        // todo refactor
        public static ProblemOutputModel MapProblemOverviewToOutputModel(ProblemOverview problemOverview)
        {
            return new ProblemOutputModel
                       {
                           Id = problemOverview.Id,
                           Name = problemOverview.Name,
                           Description = problemOverview.Description,
                           ProblemNumber = problemOverview.ProblemNumber,
                           ResponsibleUserName = problemOverview.ResponsibleUserName
                       };
        }

        public static ProblemEditOutputModel MapProblemOverviewToEditOutputModel(ProblemOverview problemOverview)
        {
            return new ProblemEditOutputModel
            {
                Id = problemOverview.Id,
                Name = problemOverview.Name,
                Description = problemOverview.Description,
                ProblemNumber = problemOverview.ProblemNumber,
                InventoryNumber = problemOverview.InventoryNumber,
                ResponsibleUserId = problemOverview.ResponsibleUserId,
                ShowOnStartPage = problemOverview.ShowOnStartPage
            };
        }

        public ActionResult Index()
        {
            var problems = this.problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id, (EntityStatus)Enums.Show.Active);
            var customers = this.customerService.GetCustomers(SessionFacade.CurrentCustomer.Id);

            var problemOutputModels = problems.Select(MapProblemOverviewToOutputModel).ToList();
            var customerOutputModels = customers.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString(CultureInfo.InvariantCulture) }).ToList();

            var viewModel = new ProblemOutputViewModel { Problems = problemOutputModels, Customers = customerOutputModels, Show = Enums.Show.Active };

            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Search(SearchInputModel searchInputModel)
        {
            var problems = this.problemService.GetCustomerProblems(searchInputModel.CustomerId, (EntityStatus)searchInputModel.Show);
            var problemOutputModels = problems.Select(MapProblemOverviewToOutputModel).ToList();

            return this.PartialView("ProblemGrid", problemOutputModels);
        }

        public ActionResult Problem(int id)
        {
            var problem = this.problemService.GetProblem(id);
            var users = this.userService.GetUsers();

            var problemOutputModel = MapProblemOverviewToEditOutputModel(problem);
            var userOutputModels = users.Select(x => new SelectListItem { Text = string.Format("{0} {1}", x.FirstName, x.SurName), Value = x.Id.ToString(CultureInfo.InvariantCulture) }).ToList();

            var viewModel = new ProblemEditOutputViewModel { Problem = problemOutputModel, Users = userOutputModels };

            return this.View(viewModel);
        }

        public ActionResult Save(ProblemEditInputModel problem)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return this.RedirectToAction("Index");
        }
    }
}
