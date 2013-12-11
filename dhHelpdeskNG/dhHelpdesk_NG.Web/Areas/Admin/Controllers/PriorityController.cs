using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Infrastructure.Extensions;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class PriorityController : BaseController
    {
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IPriorityService _priorityService;
        private readonly ICustomerService _customerService;

        public PriorityController(
            IMailTemplateService mailTemplateService,
            IPriorityService priorityService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _mailTemplateService = mailTemplateService;
            _priorityService = priorityService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var priorities = _priorityService.GetPriorities(customer.Id).ToList();

            var model = new PriorityIndexViewModel { Priorities = priorities, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var priority = new Priority { Customer_Id = customer.Id, IsActive = 1 };
            //var model = CreateInputViewModel(new Priority { Customer_Id = SessionFacade.CurrentCustomer.Id, IsActive = 1 });
            var model = CreateInputViewModel(priority, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Priority priority, HttpPostedFileBase fileUploadedName)
        {
            if(fileUploadedName != null)
            {
                var fileName = "";
                var localPath = "";
                var uploadedFile = Request.Files[0];
                fileName = Path.GetFileName(uploadedFile.FileName);
                localPath = Server.MapPath("~/App_Uploads/" + fileName);
                uploadedFile.SaveAs(localPath);

                priority.FileName = fileName;
            }

            var customer = _customerService.GetCustomer(priority.Customer_Id);

            if(!ModelState.IsValid)
            {
                
                var model = CreateInputViewModel(new Priority { Customer_Id = priority.Customer_Id }, customer);

                return View(model);
            }

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _priorityService.SavePriority(priority, out errors);

            if(errors.Count == 0)
                return RedirectToAction("index", "priority", new { customerid = priority.Customer_Id });

            
            var Vmodel = CreateInputViewModel(priority, customer);

            return View(Vmodel);
        }

        public ActionResult Edit(int id)
        {
            var priority = _priorityService.GetPriority(id);

            if(priority == null)
                return new HttpNotFoundResult("No priority found...");

            var customer = _customerService.GetCustomer(priority.Customer_Id);
            var model = CreateInputViewModel(priority, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase fileUploadedName)
        {
            Priority p = _priorityService.GetPriority(id);

            if (fileUploadedName != null)
            {
                var uploadedFile = Request.Files[0];
                var fileName = Path.GetFileName(uploadedFile.FileName);
                var localPath = Server.MapPath("~/App_Uploads/" + fileName);
                uploadedFile.SaveAs(localPath);

                p.FileName = fileName;
            }

            UpdateModel(p, "priority");
            
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _priorityService.SavePriority(p, out errors);

            if(errors.Count == 0)
                return RedirectToAction("index", "priority", new { customerid = p.Customer_Id });

            var customer = _customerService.GetCustomer(p.Customer_Id);
            var model = CreateInputViewModel(p, customer);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var priority = _priorityService.GetPriority(id);

            if (_priorityService.DeletePriority(id) == DeleteMessage.Success)
                return RedirectToAction("index", "priority", new { customerid = priority.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "priority", new { customerid = priority.Customer_Id, id = id });
            }
        }

        private PriorityInputViewModel CreateInputViewModel(Priority priority, Customer customer)
        {
            var model = new PriorityInputViewModel
            {
                Priority = priority,
                Customer = customer,
                EmailTemplates = _mailTemplateService.GetMailTemplates(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }

        [HttpPost]
        public string DeleteUploadedFile(int id)
        {
            var fileToDelete = _priorityService.GetPriority(id);

            if (fileToDelete != null)
            {
                try
                {
                    string path = Server.MapPath("~/App_Uploads/") + fileToDelete.FileName; //TODO: ändra sökväg från vart filen hämtas.. beroende på vart den kommer sparas senare, nu bara lokal väg
                    path.DeleteFile();
                    fileToDelete.FileName = "";
                }
                catch (Exception)
                {
                    throw;
                }
            }

            _priorityService.UpdateSavedFile(fileToDelete);

            return string.Empty;
        }
    }
}
