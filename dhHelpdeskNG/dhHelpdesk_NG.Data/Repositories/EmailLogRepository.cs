namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public class EmailLogRepository : RepositoryBase<EmailLog>, IEmailLogRepository
    {
        public EmailLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<EmailLog> GetEmailLogsByCaseId(int caseId)  
        {
            return (from l in this.DataContext.EmailLogs
                    join h in this.DataContext.CaseHistories on l.CaseHistory_Id equals h.Id
                    where h.Case_Id == caseId
                    select l).ToList(); 
        }

    }
}