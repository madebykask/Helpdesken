namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Problem.Output;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Models.Problem;
    using dhHelpdesk_NG.Web.Models.Problem.Input;

    public class ProblemController : BaseController
    {
        private readonly ICustomerService customerService;
        private readonly IProblemService problemService;

        public ProblemController(ICustomerService customerService, IProblemService problemService, IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.problemService = problemService;
        }

        // todo refactor
        public static ProblemInputModel MapProblemOverview(ProblemOverview problemOverview)
        {
            return new ProblemInputModel
                       {
                           Id = problemOverview.Id,
                           Name = problemOverview.Name,
                           Description = problemOverview.Description,
                           ProblemNumber = problemOverview.ProblemNumber,
                           ResponsibleUser = problemOverview.ResponsibleUser
                       };
        }

        // todo refactor
        public static List<ProblemInputModel> MapProblemOverview(IList<ProblemOverview> problemOverview)
        {
            var problemInputModels = problemOverview.Select(MapProblemOverview).ToList();
            return problemInputModels;
        }

        public ActionResult Index()
        {
            var problems = this.problemService.GetCustomerProblemOverviews(SessionFacade.CurrentCustomer.Id);
            var customers = this.customerService.GetCustomers(SessionFacade.CurrentCustomer.Id);

            var problemInputModels = MapProblemOverview(problems);
            var customerInputModels = customers.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            var states = Enum.GetValues(typeof(ProblemStates)).Cast<ProblemStates>();
            var stateInputModels = states.Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            var viewModel = new ProblemInputViewModel { Problems = problemInputModels, Customers = customerInputModels, ProblemStateses = stateInputModels };

            return this.View(viewModel);
        }
    }
}
