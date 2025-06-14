using System.Collections.Generic;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using System.Linq;

namespace DH.Helpdesk.Dal.Repositories
{
    public interface IMail2TicketRepository : IRepository<Mail2Ticket>
    {
        IList<Mail2Ticket> GetCaseMail2Tickets(int caseId);
        void DeleteByCaseId(int id);
        void DeleteByLogId(int id);
    }

    public class Mail2TicketRepository : RepositoryBase<Mail2Ticket>, IMail2TicketRepository
    {
        public Mail2TicketRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IList<Mail2Ticket> GetCaseMail2Tickets(int caseId)
        {
            var items = Table.Where(m => m.Case_Id == caseId).ToList();
            return items;
        }

        public void DeleteByCaseId(int id)
        {
            var m2ts = Table.Where(m => m.Case_Id == id).ToList();
            if (m2ts.Any())
            {
                DataContext.Mail2Tickets.RemoveRange(m2ts);
            }
        }

        public void DeleteByLogId(int id)
        {
            var m2ts = Table.Where(m => m.Log_Id == id).ToList();
            if (m2ts.Any())
            {
                DataContext.Mail2Tickets.RemoveRange(m2ts);
            }
        }
    }

}
