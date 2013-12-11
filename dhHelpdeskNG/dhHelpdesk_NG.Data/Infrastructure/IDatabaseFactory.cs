using System;

namespace dhHelpdesk_NG.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        HelpdeskDbContext Get();        
    }
}
