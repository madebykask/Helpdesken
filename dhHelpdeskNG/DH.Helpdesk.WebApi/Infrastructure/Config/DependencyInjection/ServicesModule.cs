using DH.Helpdesk.Services.BusinessLogic.Accounts;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.Concrete.Licenses;
using DH.Helpdesk.Services.Services.Concrete.Orders;
using DH.Helpdesk.Services.Services.Concrete.Reports;
using DH.Helpdesk.Services.Services.Concrete.Users;
using DH.Helpdesk.Services.Services.EmployeeService;
using DH.Helpdesk.Services.Services.EmployeeService.Concrete;
using DH.Helpdesk.Services.Services.ExtendedCase;
using DH.Helpdesk.Services.Services.Feedback;
using DH.Helpdesk.Services.Services.Invoice;
using DH.Helpdesk.Services.Services.Licenses;
using DH.Helpdesk.Services.Services.Orders;
using DH.Helpdesk.Services.Services.Reports;
using DH.Helpdesk.Services.Services.UniversalCase;
using DH.Helpdesk.Services.Services.Users;
using DH.Helpdesk.Services.Services.WebApi;
using Ninject.Modules;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    public sealed class ServicesModule : NinjectModule
    {
        #region Public Methods and Operators

        public override void Load()
        {
            //services
            Bind<IUserService>().To<UserService>();
            Bind<IHolidayService>().To<HolidayService>();
            Bind<IDepartmentService>().To<DepartmentService>();
        }

        #endregion
    }
}