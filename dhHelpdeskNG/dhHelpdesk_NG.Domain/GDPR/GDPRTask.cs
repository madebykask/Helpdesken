using System;

namespace DH.Helpdesk.Domain.GDPR
{
    public class GDPRTask
    {
        public int Id { get; set; }
        public int UserId {get; set; }
        public int FavoriteId { get; set; }
        public GDPROperationStatus Status { get; set; }
        public DateTime AddedDate { get; set; }
        public int Progress { get; set; }
    }
}