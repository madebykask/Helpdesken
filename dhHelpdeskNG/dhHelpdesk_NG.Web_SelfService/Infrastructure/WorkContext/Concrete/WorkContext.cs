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

        public ICustomerContext Customer { get; private set; }

#pragma warning disable 0618
        public ICacheContext Cache { get; private set; }
#pragma warning restore 0618

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