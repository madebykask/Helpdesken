using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Tools;
 

    public class PriorityController : BaseAdminController
    {
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IPriorityService _priorityService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly ITemporaryFilesCache userTemporaryFilesStorage;
       
        public PriorityController(
            IMailTemplateService mailTemplateService,
            IPriorityService priorityService,
            ICustomerService customerService,
            ILanguageService languageService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._mailTemplateService = mailTemplateService;
            this._priorityService = priorityService;
            this._customerService = customerService;
            this._languageService = languageService;
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Cases);
        }

        public JsonResult SetShowOnlyActivePrioritiesInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActivePrioritiesInAdmin = value;
            return this.Json(new { result = "success" });
        }


        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var priorities = this._priorityService.GetPriorities(customer.Id).ToList();

            var model = new PriorityIndexViewModel { Priorities = priorities, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActivePrioritiesInAdmin };

            return this.View(model);
        }

        [HttpPost]
        public void SortCaseSettingColumn(int customerId, string sortIds)
        {
            var elementsId = sortIds.Split('|');
            _priorityService.ReOrderPriorities(elementsId.ToList());                        
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var priority = new Priority { Customer_Id = customer.Id, IsActive = 1 };
            //var model = CreateInputViewModel(new Priority { Customer_Id = SessionFacade.CurrentCustomer.Id, IsActive = 1 });
            var model = this.CreateInputViewModel(priority, customer, null);

            return this.View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult New(Priority priority, HttpPostedFileBase fileUploadedName)
        {
            if(fileUploadedName != null)
            {
                var fileName = "";
                var localPath = "";
                var uploadedFile = this.Request.Files[0];
                fileName = Path.GetFileName(uploadedFile.FileName);
                localPath = this.Server.MapPath("~/App_Uploads/" + fileName);
                uploadedFile.SaveAs(localPath);

                priority.FileName = fileName;
            }

            var customer = this._customerService.GetCustomer(priority.Customer_Id);

            if(!this.ModelState.IsValid)
            {
                
                var model = this.CreateInputViewModel(new Priority { Customer_Id = priority.Customer_Id }, customer, null);

                return this.View(model);
            }

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._priorityService.SavePriority(priority, out errors);

            if(errors.Count == 0)
                return this.RedirectToAction("index", "priority", new { customerid = priority.Customer_Id });

            
            var Vmodel = this.CreateInputViewModel(priority, customer, null);

            return this.View(Vmodel);
        }

        public ActionResult Edit(int id)
        {
            var priority = this._priorityService.GetPriority(id);

            var priorityLanguage = this._priorityService.GetPriorityLanguage(priority.Id);

            if(priority == null)
                return new HttpNotFoundResult("No priority found...");

            var customer = this._customerService.GetCustomer(priority.Customer_Id);
            var model = this.CreateInputViewModel(priority, customer, priorityLanguage);

            return this.View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(int id, PriorityLanguage priorityLanguage, HttpPostedFileBase DownloadFile, int languageId)
        {
            Priority p = this._priorityService.GetPriority(id);

            if (DownloadFile != null)
            {
                var uploadedFile = this.Request.Files[0];
                var fileName = Path.GetFileName(uploadedFile.FileName);
                var localPath = this.Server.MapPath("~/App_Uploads/" + fileName);
                uploadedFile.SaveAs(localPath);

                p.FileName = fileName;
            }


            // check if prioritylanguage already exists 
            PriorityLanguage pl = this._priorityService.GetPriorityLanguage(id);

            if (pl != null)
            {
                pl.InformUserText = priorityLanguage.InformUserText;
                priorityLanguage = pl;
            }

            var update = true;

            
            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (pl == null)
            {
                priorityLanguage = new PriorityLanguage
                {
                    Priority_Id = id,
                    Language_Id = languageId,
                    InformUserText = priorityLanguage.InformUserText
                };

                update = false;
            }
           
            this._priorityService.SavePriorityLanguage(priorityLanguage, update, out errors);

            this.UpdateModel(p, "priority");
            
            
            this._priorityService.SavePriority(p, out errors);

            if(errors.Count == 0)
                return this.RedirectToAction("index", "priority", new { customerid = p.Customer_Id });

            var customer = this._customerService.GetCustomer(p.Customer_Id);
            var model = this.CreateInputViewModel(p, customer, null);

            return this.View(model);
        }

        public ActionResult Delete(int id)
        {
            var priority = this._priorityService.GetPriority(id);

            if (this._priorityService.DeletePriority(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "priority", new { customerid = priority.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "priority", new { customerid = priority.Customer_Id, id = id });
            }
        }

        private PriorityInputViewModel CreateInputViewModel(Priority priority, Customer customer, PriorityLanguage priorityLanguage)
        {
            var model = new PriorityInputViewModel
            {
                Priority = priority,
                Customer = customer,
                PriorityLanguage = priorityLanguage,
                EmailTemplates = this._mailTemplateService.GetMailTemplates(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name, Enums.TranslationSource.TextTranslation),
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }

        [HttpPost]
        public void UploadPriorityFile(string id, string name)
        {

            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this.userTemporaryFilesStorage.FileExists(name, id, ModuleName.Cases))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id, ModuleName.Cases);
            }
            else
            {
                if (this._priorityService.FileExists(int.Parse(id), name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                //var priority = new Priority(uploadedData, name, DateTime.Now, int.Parse(id));

                //_priorityService.AddFile(caseFileDto);
            }
        }

        [HttpPost]
        public string DeleteUploadedFile(int id)
        {
            var fileToDelete = this._priorityService.GetPriority(id);

            if (fileToDelete != null)
            {
                try
                {
                    string path = this.Server.MapPath("~/App_Uploads/") + fileToDelete.FileName; //TODO: ändra sökväg från vart filen hämtas.. beroende på vart den kommer sparas senare, nu bara lokal väg
                    path.DeleteFile();
                    fileToDelete.FileName = "";
                }
                catch (Exception)
                {
                    throw;
                }
            }

            this._priorityService.UpdateSavedFile(fileToDelete);

            return string.Empty;
        }

        
        public string UpdateLanguageList(int id, int customerId, int priorityId)
        {

            var customer = this._customerService.GetCustomer(customerId);
            var priority = this._priorityService.GetPriority(priorityId);

            var priorityLanguageToUpdate = this._priorityService.GetPriorityLanguageByLanguageId(priorityId, id);

            if (priorityLanguageToUpdate == null)
                priorityLanguageToUpdate = new PriorityLanguage
                {

                    Priority_Id = priorityId,
                    Language_Id = id,
                    InformUserText = string.Empty,

                };


            var priorityLanguage = new PriorityLanguage() { Language_Id = priorityLanguageToUpdate.Language_Id, InformUserText = priorityLanguageToUpdate.InformUserText, Priority_Id = priorityLanguageToUpdate.Priority_Id };

            var model = this.CreateInputViewModel(priority, customer, priorityLanguage);

            //model.PriorityLanguage = priorityLanguageToUpdate;
            //model.Customer = customer;

            //this.UpdateModel(model);

            //return View(model);
            var view = "~/areas/admin/views/Priority/_Input.cshtml";
            return this.RenderRazorViewToString(view, model);
        }
    }
}
