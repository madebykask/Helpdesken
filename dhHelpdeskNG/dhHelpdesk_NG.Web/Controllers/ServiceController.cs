namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class ServiceController : BaseController
    {
        #region Constructors and Destructors

        public ServiceController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        public void RememberTab(string topic, string tab)
        {
            SessionFacade.SaveActiveTab(topic, tab);
        }

        #endregion
    }
}