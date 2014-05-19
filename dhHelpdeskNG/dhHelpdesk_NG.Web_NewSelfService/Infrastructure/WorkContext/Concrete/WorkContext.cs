using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.NewSelfService.Infrastructure.WorkContext.Concrete
{
    internal sealed class WorkContext : IWorkContext
    {
        private readonly IUserContext _user;
        public IUserContext User
        {
            get { return _user; }
        }

        public ICustomerContext Customer { get; private set; }

        public ICacheContext Cache { get; private set; }

        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        public WorkContext(IUserContext userContext)
        {
            _user = userContext;
        }
    }
}