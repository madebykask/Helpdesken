using System.Collections.Generic;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public interface IMail2TicketService
    {
        IList<Mail2Ticket> GetCaseMail2Tickets(int caseId);
    }

    public class Mail2TicketService : IMail2TicketService
    {
        private readonly IMail2TicketRepository _mail2TicketRepository;

        public Mail2TicketService(IMail2TicketRepository mail2TicketRepository)
        {
            _mail2TicketRepository = mail2TicketRepository;
        }

        public IList<Mail2Ticket> GetCaseMail2Tickets(int caseId)
        {
            return _mail2TicketRepository.GetCaseMail2Tickets(caseId);
        }
    }
}