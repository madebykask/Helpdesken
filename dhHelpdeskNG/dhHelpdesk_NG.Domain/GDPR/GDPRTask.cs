using System;

namespace DH.Helpdesk.Domain.GDPR
{
    public class GDPRTask : Entity
    {
        public int CustomerId { get; set; }
        public int UserId {get; set; }
        public int FavoriteId { get; set; }
        public GDPRTaskStatus Status { get; set; }
        public DateTime AddedDate { get; set; }
        public int Progress { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}