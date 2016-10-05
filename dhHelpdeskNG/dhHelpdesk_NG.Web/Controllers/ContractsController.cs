using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.BusinessData.Models.Contract;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Controllers
{
    public class ContractsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContractCategoryService _contractCategoryService;
        private readonly ICustomerService _customerService;
        private readonly IContractService _contractService;
        private readonly ISupplierService _supplierService;
       
        public ContractsController(
            IUserService userService,
            IContractCategoryService contractCategoryService,
            ICustomerService customerService,
            IContractService contractService,
            ISupplierService supplierService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {            
            this._userService = userService;
            this._contractCategoryService = contractCategoryService;
            this._contractService = contractService;
            this._customerService = customerService;
            this._supplierService = supplierService;
        }


        //
        // GET: /Contract/

        [HttpGet]
        public ActionResult Index()
        {
            var customer = _customerService.GetCustomer(SessionFacade.CurrentCustomer.Id);
            var model = new ContractIndexViewModel(customer);
            var contractcategories = _contractCategoryService.GetContractCategories(customer.Id);
            var suppliers = _supplierService.GetActiveSuppliers(customer.Id);

            var filter = new ContractSelectedFilter();

            model.Rows = GetIndexRowModel(customer.Id, filter);

            model.ContractCategories = contractcategories.OrderBy(a => a.Name).ToList();
            model.Suppliers = suppliers.OrderBy(s => s.Name).ToList();
            model.Setting = GetSettingsModel(customer.Id);
           
            return this.View(model);
        }

        [HttpPost]
        public JsonResult SaveContractFieldsSetting(JSContractsSettingRowViewModel[] contractSettings)
        {
            int currentCustomerId;
            if (SessionFacade.CurrentCustomer == null)
                return Json(new { state = false, message = "Session Timeout. Please refresh the page!" }, JsonRequestBehavior.AllowGet);
            else
                currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var allFieldsSettingRows = _contractService.GetContractsSettingRows(currentCustomerId);
            
            try
            {
                var contractSettingModels = new List<ContractsSettingRowModel>();
                var now = DateTime.Now;
                
                for (int i = 0; i < contractSettings.Length; i++)
                {
                    var oldRow = allFieldsSettingRows.Where(s => s.ContractField.ToLower() == contractSettings[i].ContractField).FirstOrDefault();
                    var createdDate = oldRow == null ? now : oldRow.CreatedDate;
                    var id = oldRow == null ? 0 : oldRow.Id;
                    var contractSettingModel = new ContractsSettingRowModel
                    (
                        id,
                        currentCustomerId,
                        contractSettings[i].ContractField,
                        Convert.ToBoolean(contractSettings[i].Show),
                        Convert.ToBoolean(contractSettings[i].ShowInList),
                        contractSettings[i].Caption_Sv != null ? contractSettings[i].Caption_Sv : string.Empty,
                        contractSettings[i].Caption_Eng != null ? contractSettings[i].Caption_Eng : string.Empty,
                        Convert.ToBoolean(contractSettings[i].Required),
                        createdDate,
                        now
                   );

                    contractSettingModels.Add(contractSettingModel);
                }

                this._contractService.SaveContractSettings(contractSettingModels);

            }
            catch (Exception ex)
            {
                return Json(new {state = false, message=ex.Message}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { state = true, message = Translation.GetCoreTextTranslation("Saved success!") }, JsonRequestBehavior.AllowGet);
        }

        private ContractsIndexRowsModel GetIndexRowModel(int customerId, ContractSelectedFilter selectedFilter)
        {
            var customer = _customerService.GetCustomer(customerId);
            var model = new ContractsIndexRowsModel(customer);
            var allContracts = _contractService.GetContracts(customerId);
            var selectedContracts = new List<Contract>();

            if (selectedFilter.SelectedContractCategories.Any())
            {
               
            }

            

            foreach (var con in allContracts)
            {
                model.Data.Add(new ContractsIndexRowModel
                {
                    ContractId = con.Id,
                    ContractNumber = con.ContractNumber,
                    ContractEndDate = con.ContractEndDate,
                    ContractStartDate = con.ContractStartDate,
                    Finished = con.Finished,
                    Running = con.Running,
                    FollowUpInterval = con.FollowUpInterval,
                    Info = con.Info,
                    NoticeDate = con.NoticeDate,
                    Supplier = con.Supplier,
                    ContractCategory = con.ContractCategory,
                    Department = con.Department,
                    ResponsibleUser = con.ResponsibleUser,
                    FollowUpResponsibleUser = con.FollowUpResponsibleUser
                });
            }

            return model;
        }

        private ContractsSettingViewModel GetSettingsModel(int customerId)
        {
            var model = new ContractsSettingViewModel();
            model.customer_id = customerId;
            model.language_id = SessionFacade.CurrentLanguageId;

            model.Languages.Add(new SelectListItem() { Selected = true, Text = "SV", Value = "1" });
            model.Languages.Add(new SelectListItem() { Selected = false, Text = "EN", Value = "2" });

            var settingsRows = new List<ContractsSettingRowViewModel>();
            var allFieldsSettingRows = _contractService.GetContractsSettingRows(customerId);
            settingsRows = allFieldsSettingRows.Select(s => new ContractsSettingRowViewModel
            {
                Id = s.Id,
                ContractField = s.ContractField.ToLower(),
                ContractFieldLable = s.ContractFieldLable,
                ContractFieldLable_Eng = s.ContractFieldLable_Eng,
                Show = s.show,
                ShowInList = s.showInList,                
                Required = s.reguired              
            }).ToList();
            //settingsRows.Add(
            model.SettingRows = settingsRows;
            return model;


        }
        
        //
        // GET: /Contract/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Contract/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Contract/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Contract/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Contract/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contract/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Contract/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
