using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public interface IMail2TicketService
    {
        IList<Mail2Ticket> GetCaseMail2Tickets(int caseId);
        Task<List<Mail2Ticket>> GetCaseMail2TicketsAsync(int caseId);
    }

    public class Mail2TicketService : IMail2TicketService
    {
        private readonly IMail2TicketRepository _mail2TicketRepository;

        #region ctor()

        public Mail2TicketService(IMail2TicketRepository mail2TicketRepository)
        {
            _mail2TicketRepository = mail2TicketRepository;
        }

        #endregion

        public IList<Mail2Ticket> GetCaseMail2Tickets(int caseId)
        {
            var items = _mail2TicketRepository.GetMany(m => m.Case_Id == caseId).ToList();
            return items;
        }

        public Task<List<Mail2Ticket>> GetCaseMail2TicketsAsync(int caseId)
        {
            return _mail2TicketRepository.GetMany(m => m.Case_Id == caseId).AsQueryable().ToListAsync();
        }
    }
}