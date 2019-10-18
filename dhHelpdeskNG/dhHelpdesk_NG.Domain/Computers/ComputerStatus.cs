using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Domain.Computers
{
    public class ComputerStatus: Entity
    {
        public string Name { get; set; }
        public ComputerStatusType Type { get; set; }
        public int Customer_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }

    public enum ComputerStatusType
    {
        Computer = 1,
        Contract = 2
    }
}
