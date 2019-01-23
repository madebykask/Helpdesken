using System.Web.Mvc;

namespace DH.Helpdesk.Web.Controllers
{
    using System;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public abstract class UserInteractionController : BaseController
    {
        #region Fields

        private OperationContext _operationContext;

        #endregion

        #region Constructors and Destructors

        protected UserInteractionController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
            //Moved code to OnActionExecuting - left this part for legacy support
            if (SessionFacade.CurrentCustomer == null ||
                SessionFacade.CurrentUser == null)
            {
                return;
            }

            _operationContext = new OperationContext
                                    {
                                        CustomerId = SessionFacade.CurrentCustomer.Id,
                                        DateAndTime = DateTime.Now,
                                        LanguageId = SessionFacade.CurrentLanguageId,
                                        UserId = SessionFacade.CurrentUser.Id
                                    };
        }

        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (SessionFacade.CurrentCustomer == null || SessionFacade.CurrentUser == null)
            {
                return;
            }

            _operationContext = new OperationContext
            {
                CustomerId = SessionFacade.CurrentCustomer.Id,
                DateAndTime = DateTime.Now,
                LanguageId = SessionFacade.CurrentLanguageId,
                UserId = SessionFacade.CurrentUser.Id
            };

        }

        #region Properties

        protected OperationContext OperationContext
        {
            get
            {
                return _operationContext;
            }
            private set { _operationContext = value; }
        }

        #endregion
    }
}