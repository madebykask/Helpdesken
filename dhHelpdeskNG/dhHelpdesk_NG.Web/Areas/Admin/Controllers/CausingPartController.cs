// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models.CausingPart;
    using DH.Helpdesk.Web.Infrastructure;

    /// <summary>
    /// The causing part controller.
    /// </summary>
    public class CausingPartController : BaseAdminController
    {
        /// <summary>
        /// The customer service.
        /// </summary>
        private readonly ICustomerService customerService;

        /// <summary>
        /// The causing part service.
        /// </summary>
        private readonly ICausingPartService causingPartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CausingPartController"/> class.
        /// </summary>
        /// <param name="masterDataService">
        /// The master data service.
        /// </param>
        /// <param name="customerService">customer service</param>
        /// <param name="causingPartService">causing part service</param>
        public CausingPartController(
            IMasterDataService masterDataService, 
            ICustomerService customerService, 
            ICausingPartService causingPartService)
            : base(masterDataService)
        {
            this.customerService = customerService;
            this.causingPartService = causingPartService;
        }


        public JsonResult SetShowOnlyActiveCausingPartsInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveCausingPartsInAdmin = value;
            return this.Json(new { result = "success" });
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="ViewResult"/>.
        /// </returns>
        //[HttpGet]
        public ViewResult Index(int customerId)
        {
            var model = new CausingPartsViewModel();
            model.Customer = this.customerService.GetCustomer(customerId);
            model.CausingParts = this.causingPartService.GetCausingParts(customerId);
            model.IsShowOnlyActive = SessionFacade.ShowOnlyActiveCausingPartsInAdmin;


            return this.View(model);
        }

        /// <summary>
        /// The new.
        /// </summary>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ViewResult New(int? parentId, int customerId)
        {
            var causingPart = new CausingPartOverview();
            causingPart.CustomerId = customerId;
            causingPart.ParentId = parentId;
            causingPart.IsActive = true;
            var model = this.GetViewModel(causingPart, customerId);

            return this.View(model);
        }

        /// <summary>
        /// The new.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult New(CausingPartViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.causingPartService.SaveCausingPart(model.CausingPart);
                return this.RedirectToAction("Index", new { customerId = model.CausingPart.CustomerId });
            }
            return this.View(model);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var causingPart = this.causingPartService.GetCausingPart(id);
            var model = this.GetViewModel(causingPart, causingPart.CustomerId);
            return this.View(model);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult Edit(CausingPartViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.causingPartService.SaveCausingPart(model.CausingPart);
                return this.RedirectToAction("Index", new { customerId = model.CausingPart.CustomerId });
            }
            return this.View(model);            
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var causingPart = this.causingPartService.GetCausingPart(id);
            this.causingPartService.DeleteCausingPart(id);
            return this.RedirectToAction("Index", new { customerId = causingPart.CustomerId });
        }

        /// <summary>
        /// The get view model.
        /// </summary>
        /// <param name="causingPart">
        /// The pausing part.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="CausingPartViewModel"/>.
        /// </returns>
        private CausingPartViewModel GetViewModel(CausingPartOverview causingPart, int customerId)
        {
            var model = new CausingPartViewModel();
            model.Customer = this.customerService.GetCustomer(customerId);
            model.CausingPart = causingPart;
            return model;
        }
    }
}
