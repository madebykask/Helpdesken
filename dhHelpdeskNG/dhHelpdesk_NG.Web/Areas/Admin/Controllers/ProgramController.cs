using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class ProgramController : BaseController
    {
        private readonly IProgramService _programService;
        private readonly ICustomerService _customerService;

        public ProgramController(
            IProgramService programService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _programService = programService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var programs = _programService.GetPrograms(customer.Id);

            var model = new ProgramIndexViewModel { Programs = programs, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var program = new Program { Customer_Id = customer.Id, IsActive = 1 };
            var model = CreateInputViewModel(program, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(ProgramInputViewModel programInputViewModel)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _programService.SaveProgram(returnProgramForSave(programInputViewModel), out errors);

            var program = _programService.GetProgram(programInputViewModel.Program.Id);
            if (errors.Count == 0)
                return RedirectToAction("index", "program", new { customerId = program.Customer_Id });

            
            //var customer = _customerService.GetCustomer(program.Customer_Id);
            return View(programInputViewModel);
        }

        public ActionResult Edit(int id)
        {
            var program = _programService.GetProgram(id);

            if (program == null)
                return new HttpNotFoundResult("No program found...");

            var customer = _customerService.GetCustomer(program.Customer_Id);
            var model = CreateInputViewModel(program, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProgramInputViewModel programInputViewModel)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var program = returnProgramForSave(programInputViewModel);

            _programService.SaveProgram(program, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "program", new { customerId = program.Customer_Id });

            var customer = _customerService.GetCustomer(program.Customer_Id);
            var model = CreateInputViewModel(program, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var program = _programService.GetProgram(id);
            if (_programService.DeleteProgram(id) == DeleteMessage.Success)
                return RedirectToAction("index", "program", new { customerId = program.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "program", new { area = "admin", id = program.Id });
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
