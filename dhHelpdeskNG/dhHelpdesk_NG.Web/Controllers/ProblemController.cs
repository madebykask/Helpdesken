namespace dhHelpdesk_NG.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Models.Problem;
    using dhHelpdesk_NG.Web.Models.Problem.Input;
    using dhHelpdesk_NG.Web.Models.Problem.Output;

    public class ProblemsController : BaseController
    {
        private readonly ICustomerService customerService;
        private readonly IProblemService problemService;

        public ProblemsController(ICustomerService customerService, IProblemService problemService, IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.problemService = problemService;
        }

        // todo refactor
        public static ProblemOutputModel MapProblemOverview(ProblemOverview problemOverview)
        {
            return new ProblemOutputModel
                       {
                           Id = problemOverview.Id,
                           Name = problemOverview.Name,
                           Description = problemOverview.Description,
                           ProblemNumber = problemOverview.ProblemNumber,
                           ResponsibleUser = problemOverview.ResponsibleUser
                       };
        }

        // todo refactor
        public static List<ProblemOutputModel> MapProblemOverview(IList<ProblemOverview> problemOverview)
        {
            var problemInputModels = problemOverview.Select(MapProblemOverview).ToList();
            return problemInputModels;
        }

        public ActionResult Index()
        {
            var problems = this.problemService.FindByCustomerId(SessionFacade.CurrentCustomer.Id);
            var customers = this.customerService.GetCustomers(SessionFacade.CurrentCustomer.Id);

            var problemInputModels = MapProblemOverview(problems);
            var customerInputModels = customers.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            var viewModel = new ProblemInputViewModel { Problems = problemInputModels, Customers = customerInputModels };

            return this.View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Search(SearchInputModel searchInputModel)
        {
            var problems = this.problemService.FindByCustomerIdAndStatus(searchInputModel.CustomerId, (EntityStatus)searchInputModel.Show);
            var problemInputModels = MapProblemOverview(problems);

            return this.PartialView("ProblemGrid", problemInputModels);
        }

        public ActionResult Problem(int id)
        {
            var problem = this.problemService.FindById(id);
            var problemInputModel = MapProblemOverview(problem);

            return this.View(problemInputModel);
        }
    }
}
