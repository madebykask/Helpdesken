using System;

namespace DH.Helpdesk.EForm.Model.Entities
{
    public class GlobalView
    {
        public int Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public int CompanyId { get; set; }
        public string Unit { get; set; }
        public int UnitId { get; set; }
        public string Department { get; set; }    
        public string Function { get; set; }
        public string CaseNumber { get; set; }
        public string Email { get; set; } 
        public DateTime? RegTime { get; set; }

        public string IKEANetworkID { get; set; }
        //public string WatchDate { get; set; }

    }
}
