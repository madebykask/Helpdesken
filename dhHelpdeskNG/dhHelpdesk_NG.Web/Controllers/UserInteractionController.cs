namespace DH.Helpdesk.Web.Controllers
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public abstract class UserInteractionController : BaseController
    {
        #region Fields

        private readonly OperationContext operationContext;

        #endregion

        #region Constructors and Destructors

        protected UserInteractionController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this.operationContext = new OperationContext
                                    {
                                        CustomerId = SessionFacade.CurrentCustomer.Id,
                                        DateAndTime = DateTime.Now,
                                        LanguageId = SessionFacade.CurrentLanguageId,
                                        UserId = SessionFacade.CurrentUser.Id
                                    };
        }

        #endregion

        #region Properties

        protected OperationContext OperationContext
        {
            get
            {
                return this.operationContext;
            }
        }

        #endregion
    }
}