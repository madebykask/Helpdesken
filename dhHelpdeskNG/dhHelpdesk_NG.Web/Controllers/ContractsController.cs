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
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Web.Infrastructure.Tools;
using System.Net;
using DH.Helpdesk.Dal.Enums;
using System.IO;

namespace DH.Helpdesk.Web.Controllers
{
    public class ContractsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContractCategoryService _contractCategoryService;
        private readonly ICustomerService _customerService;
        private readonly IContractService _contractService;
        private readonly ISupplierService _supplierService;
        private readonly IDepartmentService _departmentService;
        private readonly ITemporaryFilesCache userTemporaryFilesStorage;


        public ContractsController(
            IUserService userService,
            IContractCategoryService contractCategoryService,
            ICustomerService customerService,
            IContractService contractService,
            ISupplierService supplierService,
            IDepartmentService departmentService,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {            
            this._userService = userService;
            this._contractCategoryService = contractCategoryService;
            this._contractService = contractService;
            this._customerService = customerService;
            this._supplierService = supplierService;
            this._departmentService = departmentService;
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Contracts);
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

            model.Rows = GetIndexRowModel(customer.Id, filter, new ColSortModel(EnumContractFieldSettings.Number, true));

            model.ContractCategories = contractcategories.OrderBy(a => a.Name).ToList();
            model.Suppliers = suppliers.OrderBy(s => s.Name).ToList();
            model.Setting = GetSettingsModel(customer.Id);
           
            return this.View(model);
        }
         
        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            //var accountAcctivity = new AccountActivity { Customer_Id = customer.Id };
            var model = this.CreateInputViewModel(customerId);

            return this.View(model);
        }

        private ContractViewInputModel CreateInputViewModel(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var model = new ContractViewInputModel();
            var contractFields = this.GetSettingsModel(customerId);            
            var contractcategories = _contractCategoryService.GetContractCategories(customerId).OrderBy(a => a.Name).ToList();
            var suppliers = _supplierService.GetActiveSuppliers(customerId);
            var departments = _departmentService.GetDepartments(customerId);
            var users = _userService.GetCustomerActiveUsers(customerId);

            var emptyChoice = new SelectListItem() { Selected = true, Text = "", Value = string.Empty };

            //if (contractFields != null)
            //{          
                //model.NoticeTime.Insert(0, emptyChoice);
                model.NoticeTimes.Add(new SelectListItem() { Selected = false, Text = "1 månad", Value = "1" });

                for (int i = 2; i <= 12; i++)
                {
                    model.NoticeTimes.Add(new SelectListItem() { Selected = false, Text = i.ToString() + " månader", Value = i.ToString() });
                }
                                       
                model.SettingsModel = contractFields.SettingRows.Select(conf => new ContractsSettingRowViewModel
                    {
                        Id = conf.Id,
                        ContractField = conf.ContractField.ToLower(),
                        ContractFieldLable = conf.ContractFieldLable,
                        ContractFieldLable_Eng = conf.ContractFieldLable_Eng,
                        Show = conf.Show,
                        ShowInList = conf.ShowInList,
                        Required = conf.Required
                    }).ToList();

                model.ContractFiles = new List<ContractFileViewModel>();
                model.ContractFileKey = Guid.NewGuid().ToString();

            model.ContractCategories = contractcategories.Select(x => new SelectListItem
                {
                    Selected = (x.Id == model.CategoryId ? true : false),
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                model.ContractCategories.Insert(0, emptyChoice);

                model.Suppliers = suppliers.Select(x => new SelectListItem
                {
                    Selected = (x.Id == model.SupplierId ? true : false),
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                model.Suppliers.Insert(0, emptyChoice);

                model.Departments = departments.Select(x => new SelectListItem
                {
                    Selected = (x.Id == model.DepartmentId ? true : false),
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                }).ToList();
                model.Departments.Insert(0, emptyChoice);


                model.ResponsibleUsers = users.Select(x => new SelectListItem
                {
                    Selected = (x.Id == model.ResponsibleUserId ? true : false),
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList();
                model.ResponsibleUsers.Insert(0, emptyChoice);

                model.FollowUpIntervals.Insert(0, emptyChoice);
                model.FollowUpIntervals.Add(new SelectListItem() { Selected = false, Text = "månadsvis", Value = "1" });
                model.FollowUpIntervals.Add(new SelectListItem() { Selected = false, Text = "kvartalsvis", Value = "3" });
                model.FollowUpIntervals.Add(new SelectListItem() { Selected = false, Text = "tertialvis", Value = "4" });
                model.FollowUpIntervals.Add(new SelectListItem() { Selected = false, Text = "halvårsvis", Value = "6" });
                model.FollowUpIntervals.Add(new SelectListItem() { Selected = false, Text = "årsvis", Value = "12" });



                model.FollowUpResponsibleUsers = users.Select(x => new SelectListItem
                {
                    Selected = (x.Id == model.FollowUpResponsibleUserId ? true : false),
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList();
                model.FollowUpResponsibleUsers.Insert(0, emptyChoice);

            //}


            return model;
        }

        [HttpPost]
        public ActionResult New(ContractViewInputModel contractInput, string actiontype, string contractFileKey)
        {
            var cId = this.SaveContract(contractInput);
            var temporaryFiles = userTemporaryFilesStorage.FindFiles(contractFileKey.ToString());
            var contractFiles = temporaryFiles.Select(f => new ContractFileModel(0, cId, f.Content, null, "", f.Name, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid())).ToList();

            foreach(var contractFile in contractFiles)
            {
                this._contractService.SaveContracFile(contractFile);
                userTemporaryFilesStorage.DeleteFile(contractFile.FileName, contractInput.ContractFileKey);
            }            

            if (actiontype != "Spara och stäng")
            {             
                return this.RedirectToAction("Edit", "Contracts", new { id = cId });
            }

                     
           // this.SaveContractFiles(mmm, cId);
            
            return RedirectToAction("index", "Contracts");            
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

        public ActionResult SortBy(int customerId, string colName, bool isAsc)
        {
            var model = GetIndexRowModel(customerId, null, new ColSortModel(colName, isAsc));
            return PartialView("_ContractsIndexRows", model);
        }

        private ContractsIndexRowsModel GetIndexRowModel(int customerId, ContractSelectedFilter selectedFilter, ColSortModel sort)
        {
            var customer = _customerService.GetCustomer(customerId);
            var model = new ContractsIndexRowsModel(customer);           
            var allContracts = _contractService.GetContracts(customerId);
            var settings = GetSettingsModel(customer.Id);
            var selectedContracts = new List<Contract>();

            if (selectedFilter != null && selectedFilter.SelectedContractCategories.Any())
            {
               
            }

            model.Columns = settings.SettingRows.Where(s => s.ShowInList == true)
                                                .ToList();

            foreach (var col in model.Columns)
                col.SetOrder();

            model.Columns = model.Columns.OrderBy(s => s.VirtualOrder).ToList();

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

            model.Data = SortData(model.Data, sort);
            model.SortBy = sort;
                 
            return model;
        }

        private List<ContractsIndexRowModel> SortData(List<ContractsIndexRowModel> data, ColSortModel sort)
        {
            switch (sort.ColumnName)
            {
                case EnumContractFieldSettings.Number:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractNumber).ToList() : data.OrderByDescending(d => d.ContractNumber).ToList();

                case EnumContractFieldSettings.CaseNumber:
                    return sort.IsAsc ? data.OrderBy(d => d.CaseNumber).ToList() : data.OrderByDescending(d => d.CaseNumber).ToList();

                case EnumContractFieldSettings.Category:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractCategory.Name).ToList() : data.OrderByDescending(d => d.ContractCategory.Name).ToList();

                case EnumContractFieldSettings.Supplier:                   
                    return sort.IsAsc ? data.OrderBy(t => t.Supplier != null && t.Supplier.Name != null
                                         ? t.Supplier.Name : string.Empty).ToList() :
                                        data.OrderByDescending(t => t.Supplier != null && t.Supplier.Name != null
                                         ? t.Supplier.Name : string.Empty).ToList();

                case EnumContractFieldSettings.Department:
                    return sort.IsAsc ? data.OrderBy(d => d.Department.DepartmentName).ToList() : data.OrderByDescending(d => d.Department.DepartmentName).ToList();

                case EnumContractFieldSettings.ResponsibleUser:
                    return sort.IsAsc ? data.OrderBy(t => t.ResponsibleUser != null && t.ResponsibleUser.SurName != null
                                         ? t.ResponsibleUser.SurName : string.Empty).ToList() : 
                                        data.OrderByDescending(t => t.ResponsibleUser != null && t.ResponsibleUser.SurName != null
                                         ? t.ResponsibleUser.SurName : string.Empty).ToList();

                case EnumContractFieldSettings.StartDate:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractStartDate).ToList() : data.OrderByDescending(d => d.ContractStartDate).ToList();

                case EnumContractFieldSettings.EndDate:
                    return sort.IsAsc ? data.OrderBy(d => d.ContractEndDate).ToList() : data.OrderByDescending(d => d.ContractEndDate).ToList();

                case EnumContractFieldSettings.NoticeDate:
                    return sort.IsAsc ? data.OrderBy(d => d.NoticeDate).ToList() : data.OrderByDescending(d => d.NoticeDate).ToList();

                //case EnumContractFieldSettings.Filename:
                //    return sort.IsAsc ? data.OrderBy(d => d.).ToList() : data.OrderByDescending(d => d.).ToList();

                case EnumContractFieldSettings.Other:
                    return sort.IsAsc ? data.OrderBy(d => d.Info).ToList() : data.OrderByDescending(d => d.Info).ToList();

                case EnumContractFieldSettings.Running:
                    return sort.IsAsc ? data.OrderBy(d => d.Running).ToList() : data.OrderByDescending(d => d.Running).ToList();

                case EnumContractFieldSettings.Finished:
                    return sort.IsAsc ? data.OrderBy(d => d.Finished).ToList() : data.OrderByDescending(d => d.Finished).ToList();

                case EnumContractFieldSettings.FollowUpField:
                    return sort.IsAsc ? data.OrderBy(d => d.FollowUpInterval).ToList() : data.OrderByDescending(d => d.FollowUpInterval).ToList();

                case EnumContractFieldSettings.ResponsibleFollowUpField:
                    //return sort.IsAsc ? data.OrderBy(d => d.FollowUpResponsibleUser).ToList() : data.OrderByDescending(d => d.FollowUpResponsibleUser).ToList();
                    return sort.IsAsc ? data.OrderBy(t => t.FollowUpResponsibleUser != null && t.FollowUpResponsibleUser.SurName != null
                                        ? t.FollowUpResponsibleUser.SurName : string.Empty).ToList() :
                                        data.OrderByDescending(t => t.FollowUpResponsibleUser != null && t.FollowUpResponsibleUser.SurName != null
                                        ? t.FollowUpResponsibleUser.SurName : string.Empty).ToList();
            }

            return data;
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


        [HttpPost]
        public void UploadContractFile(string id, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(id))
            {
                if (this.userTemporaryFilesStorage.FileExists(name, id))
                {
                    //return;
                    //this.userTemporaryFilesStorage.DeleteFile(name, id, ModuleName.Log); 
                    //throw new HttpException((int)HttpStatusCode.Conflict, null); because it take a long time.
                }
                this.userTemporaryFilesStorage.AddFile(uploadedData, name, id);
            }
        }
       

        [HttpGet]
        public FileContentResult DownloadContractFile(string id, string fileName)
        {
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
                fileContent = userTemporaryFilesStorage.GetFileContent(fileName, id, "");
            else
            {
                
                //fileContent = _caseFileService.GetFileContentByIdAndFileName(int.Parse(id), basePath, fileName);
                fileContent = null;
            }
            return File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public JsonResult DeleteContractFile(string id, string fileName, string filePlace)
        {
            try
            {
                if (id == "0")
                    userTemporaryFilesStorage.DeleteFile(fileName.Trim(), filePlace);
                else
                {

                }
                return Json(new { result = true, message = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
            
        }

        [HttpGet]
        public JsonResult GetAllFiles(string id , int cId)
        {
            string[] fileNames = { };           
            var tempFiles = userTemporaryFilesStorage.FindFiles(id);

            if (tempFiles.Any())
            {
                fileNames = tempFiles.Select(f => f.Name).ToArray();
            }
         
            return Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        private int SaveContract(ContractViewInputModel contractInput)
        {
            var cId = 0;
            if (contractInput != null)
            {
                var contractInputToSave = new ContractInputModel
                    (
                     contractInput.ContractId,
                     SessionFacade.CurrentCustomer.Id,
                     SessionFacade.CurrentUser.Id,
                     contractInput.CategoryId,
                     contractInput.SupplierId,
                     contractInput.DepartmentId,
                     contractInput.ResponsibleUserId,
                     contractInput.FollowUpResponsibleUserId,
                     contractInput.ContractNumber,
                     contractInput.ContractStartDate,
                     contractInput.ContractEndDate,
                     contractInput.NoticeTimeId,
                     contractInput.Finished,
                     contractInput.Running,
                     contractInput.FollowUpIntervalId,
                     contractInput.Other,
                     contractInput.NoticeDate,
                     DateTime.Now,
                     DateTime.Now,
                     Guid.Empty
                    );

                cId = this._contractService.SaveContract(contractInputToSave);
            }

            return cId;
        }
        private void SaveContractFiles(List<ContractFileViewModel> contractFilesInputs, int contractId)
        {
            foreach (var contractFile in contractFilesInputs)
            {
                var contractFileInputToSave = new ContractFileModel
                    (
                     0,
                     contractId,
                     contractFile.Content,
                     0,
                     contractFile.ContractType,
                     contractFile.FileName,
                     DateTime.UtcNow,                                         
                     DateTime.UtcNow,
                     Guid.Empty
                    );

              this._contractService.SaveContracFile(contractFileInputToSave);
            }           
        }

        private List<ContractFileViewModel> CreateContractFilesModel(int contractId)
        {
            var contractFiles = this._contractService.GetContractFiles(contractId);
            var contractFilesInput = contractFiles.Select(conf => new ContractFileViewModel()
            {
                Id = conf.Id,
                Contract_Id = conf.Contract_Id,
                ArchivedContractFile_Id = conf.ArchivedContractFile_Id,
                FileName = conf.FileName,
                ArchivedDate = conf.ArchivedDate,
                Content = conf.Content,
                ContractType = conf.ContentType,
                ContractFileGuid = conf.ContractFileGuid,
                CreatedDate = conf.CreatedDate
            }).ToList();
            return contractFilesInput;
        }

        private List<string> ContractFilesNames(int contractId)
        {
            List<string> contractFilesNames = null;
            var contractFiles = this._contractService.GetContractFiles(contractId);
            contractFilesNames = contractFiles.Select(conf => conf.FileName).ToList();

            return contractFilesNames;
        }

        private List<WebTemporaryFile> TransferContractFiles(string contractFilesKey, int contractId)
        {
            var filesToAdd = new List<WebTemporaryFile>();
            var contractFiles = this._contractService.GetContractFiles(contractId);
            if (contractFiles != null)
            {
                foreach (var f in contractFiles)
                {
                    filesToAdd.Add(new WebTemporaryFile(f.Content, f.FileName));
                    this.userTemporaryFilesStorage.AddFile(f.Content, f.FileName, contractFilesKey);
                }                
            }
            return filesToAdd;
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
            var contractFields = this.GetSettingsModel(SessionFacade.CurrentCustomer.Id);
            var contract = this._contractService.GetContract(id);


            if (contract == null)
                return new HttpNotFoundResult("No contract found...");
            else
            {
                var contractsExistingFiles = this._contractService.GetContractFiles(id);
                var changedbyUser = this._userService.GetUser(contract.ChangedByUser_Id);

                var contractEditInput = CreateInputViewModel(SessionFacade.CurrentCustomer.Id);

                contractEditInput.ContractId = contract.Id;
                contractEditInput.CategoryId = contract.ContractCategory_Id;
                contractEditInput.SupplierId = Convert.ToInt32(contract.Supplier_Id);
                contractEditInput.DepartmentId = Convert.ToInt32(contract.Department_Id);
                contractEditInput.ResponsibleUserId = Convert.ToInt32(contract.ResponsibleUser_Id);
                contractEditInput.FollowUpIntervalId = contract.FollowUpInterval;
                contractEditInput.FollowUpResponsibleUserId = Convert.ToInt32(contract.FollowUpResponsibleUser_Id);
                contractEditInput.ContractNumber = contract.ContractNumber;
                contractEditInput.ContractStartDate = contract.ContractStartDate;
                contractEditInput.ContractEndDate = contract.ContractEndDate;
                contractEditInput.NoticeTimeId = contract.NoticeTime;
                contractEditInput.Finished = Convert.ToBoolean(contract.Finished);
                contractEditInput.Running = Convert.ToBoolean(contract.Running);
                contractEditInput.Other = contract.Info;
                contractEditInput.NoticeDate = contract.NoticeDate;
                contractEditInput.ChangedByUser = changedbyUser.SurName + " " + changedbyUser.FirstName;
                contractEditInput.CreatedDate = contract.CreatedDate.ToShortDateString();
                contractEditInput.ChangedDate = contract.ChangedDate.ToShortDateString();
                contractEditInput.ContractFiles = CreateContractFilesModel(contract.Id);

                return this.View(contractEditInput);
            }
        }


        //
        // POST: /Contract/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ContractViewInputModel contractInput , string actiontype , string contractFileKey)
        {
            try
            {                
                var cId = SaveContract(contractInput);
                var temporaryFiles = userTemporaryFilesStorage.FindFiles(contractFileKey);
                var contractsExistingFiles = this._contractService.GetContractFiles(id);
                var contractFiles = temporaryFiles.Select(f => new ContractFileModel(0, cId, f.Content, null, "", f.Name, DateTime.UtcNow, DateTime.UtcNow, Guid.NewGuid())).ToList();

                foreach (var contractFile in contractFiles)
                {
                    this._contractService.SaveContracFile(contractFile);
                    userTemporaryFilesStorage.DeleteFile(contractFile.FileName, contractInput.ContractFileKey);
                }

                if (actiontype != "Spara och stäng")
                {
                    return this.RedirectToAction("Edit", "Contracts", new { id = cId });
                }
                else
                    return this.RedirectToAction("index", "Contracts");
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("index", "Contracts");
            }
        }

        //
        // GET: /Contract/Delete/5
 
        public ActionResult Delete(int id)
        {          
            return this.RedirectToAction("index", "Contracts");
            //return View();
        }

        //
        // POST: /Contract/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var contract = this._contractService.GetContract(id);

                this._contractService.DeleteContract(contract);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
