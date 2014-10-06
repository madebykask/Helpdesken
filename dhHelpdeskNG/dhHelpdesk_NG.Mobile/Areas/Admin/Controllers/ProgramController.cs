namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;

    public class ProgramController : BaseAdminController
    {
        private readonly IProgramService _programService;
        private readonly ICustomerService _customerService;

        public ProgramController(
            IProgramService programService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._programService = programService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var programs = this._programService.GetPrograms(customer.Id);

            var model = new ProgramIndexViewModel { Programs = programs, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var program = new Program { Customer_Id = customer.Id, IsActive = 1 };
            var model = this.CreateInputViewModel(program, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ProgramInputViewModel programInputViewModel)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._programService.SaveProgram(this.returnProgramForSave(programInputViewModel), out errors);

            var program = this._programService.GetProgram(programInputViewModel.Program.Id);
            if (errors.Count == 0)
                return this.RedirectToAction("index", "program", new { customerId = program.Customer_Id });

            
            //var customer = _customerService.GetCustomer(program.Customer_Id);
            return this.View(programInputViewModel);
        }

        public ActionResult Edit(int id)
        {
            var program = this._programService.GetProgram(id);

            if (program == null)
                return new HttpNotFoundResult("No program found...");

            var customer = this._customerService.GetCustomer(program.Customer_Id);
            var model = this.CreateInputViewModel(program, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProgramInputViewModel programInputViewModel)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var program = this.returnProgramForSave(programInputViewModel);

            this._programService.SaveProgram(program, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "program", new { customerId = program.Customer_Id });

            var customer = this._customerService.GetCustomer(program.Customer_Id);
            var model = this.CreateInputViewModel(program, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var program = this._programService.GetProgram(id);
            if (this._programService.DeleteProgram(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "program", new { customerId = program.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "program", new { area = "admin", id = program.Id });
            }
        }

        private ProgramInputViewModel CreateInputViewModel(Program program, Customer customer)
        {
            var model = new ProgramInputViewModel()
            {
                Program = program,
                Customer = customer
            };

            if (program.Id == 0)
            {
                model.ShowOrder = 0;
                model.ShowAccount = 0;
            }
            else
            {
                if (program.ShowOnStartPage == 3)
                {
                    model.ShowOrder = 1;
                    model.ShowAccount = 1;
                }
                else if (program.ShowOnStartPage == 2)
                {
                    model.ShowOrder = 0;
                    model.ShowAccount = 1;
                }
                else if (program.ShowOnStartPage == 1)
                {
                    model.ShowOrder = 1;
                    model.ShowAccount = 0;
                }
                else
                {
                    model.ShowOrder = 0;
                    model.ShowAccount = 0;
                }
            }

            return model;
        }

        private Program returnProgramForSave(ProgramInputViewModel programInputViewModel)
        {
            var program = programInputViewModel.Program;

            if (programInputViewModel.ShowOrder == 1 && programInputViewModel.ShowAccount == 1)
            {
                program.ShowOnStartPage = 3;
            }
            else if (programInputViewModel.ShowOrder == 0 && programInputViewModel.ShowAccount == 1)
            {
                program.ShowOnStartPage = 2;
            }
            else if (programInputViewModel.ShowOrder == 1 && programInputViewModel.ShowAccount == 0)
            {
                program.ShowOnStartPage = 1;
            }
            else
            {
                program.ShowOnStartPage = 0;
            }

            return program;
        }
    }
}
