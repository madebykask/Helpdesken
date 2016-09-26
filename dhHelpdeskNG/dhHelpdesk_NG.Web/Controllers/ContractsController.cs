using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Controllers
{
    public class ContractsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IContractCategoryService _contractCategoryService;
        private readonly ICustomerService _customerService;
        private readonly IContractService _contractService;
       
        public ContractsController(
            IUserService userService,
            IContractCategoryService contractCategoryService,
            ICustomerService customerService,
            IContractService contractService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {            
            this._userService = userService;
            this._contractCategoryService = contractCategoryService;
            this._contractService = contractService;
            this._customerService = customerService;
        }


        //
        // GET: /Contract/

        [HttpGet]
        public ActionResult Index()
        {
            var customer = _customerService.GetCustomer(SessionFacade.CurrentCustomer.Id);
            var model = new ContractIndexViewModel(customer);
            var contractcategories = _contractCategoryService.GetContractCategories(customer.Id);
            //var filter = new InvoiceArticleProductAreaSelectedFilter();

            model.Rows = GetIndexRowModel(customer.Id);

            model.ContractCategories = contractcategories.OrderBy(a => a.Name).ToList();
           
            return this.View(model);
        }

        private ContractsIndexRowsModel GetIndexRowModel(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var model = new ContractsIndexRowsModel(customer);

            var allContracts = _contractService.GetContractsWithCategories(customerId);

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
