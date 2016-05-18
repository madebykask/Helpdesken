using System;

namespace DH.Helpdesk.Domain
{
    public class ReportFavorite : Entity
    {
        public int Customer_Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Filters { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
