using DH.Helpdesk.Domain;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DH.Helpdesk.BusinessData.Models.ADFS.Input;
using DH.Helpdesk.Common.Types;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;

namespace DH.Helpdesk.SelfService.Controllers
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.SelfService.Models;
    using DH.Helpdesk.SelfService.Models.Case;
    using DH.Helpdesk.SelfService.Models.Start;
    using DH.Helpdesk.Dal;
    using System.Security.Claims;
    using System.Configuration;
    using DH.Helpdesk.Common.Enums;
    
    public class StartController : BaseController
    {
        private readonly ICustomerService _customerService;                
        private readonly IBulletinBoardService _bulletinBoardService;
        private readonly IInfoService _infoService;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;

        public StartController(IMasterDataService masterDataService,
                               ICustomerService customerService,
                               ICaseSolutionService caseSolutionService,
                               IInfoService infoService,                               
                               IBulletinBoardService bulletinBoardService,
                               ISettingService settingService,
                               IUserService userService,
                               ISelfServiceConfigurationService configurationService
                              ):base(configurationService, masterDataService, caseSolutionService)
        {
            
            this._customerService = customerService;
            this._bulletinBoardService = bulletinBoardService;
            this._infoService = infoService;
            this._settingService = settingService;
            this._userService = userService;
        }

        //
        // GET: /Start/

        public  ActionResult Index(int customerId = -1)
        {
            var htmlData = _infoService.GetInfoText((int)InfoTextType.SelfServiceWelcome, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            var model = new StartPageModel(htmlData == null ? string.Empty : htmlData.Name);
            var customerSetting = this._settingService.GetCustomerSettings(SessionFacade.CurrentCustomer.Id);

            var bb = _bulletinBoardService.GetBulletinBoards(SessionFacade.CurrentCustomer.Id, false);
            model.BulletinBoard = bb.Where(b => b.PublicInformation != 0 &&
                                                b.ShowDate <= DateTime.Now.Date && b.ShowUntilDate >= DateTime.Now.Date)
                                    .OrderByDescending(b => b.ShowDate)
                                    .ToList();

            return this.View(model);                        
         }        
    }
}
