using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.SelfService.Infrastructure.WorkContext.Concrete
{
    internal sealed class WorkContext : IWorkContext
    {
        private readonly IUserContext _user;
        public IUserContext User
        {
            get { return _user; }
        }

        public WorkContext(IUserContext userContext)
        {
            _user = userContext;
        }
    }
}