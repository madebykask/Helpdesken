namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
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