namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;

    using DH.Helpdesk.Dal.DbContext;

    public interface IDatabaseFactory : IDisposable
    {
        HelpdeskDbContext Get();        
    }
}
