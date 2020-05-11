using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.ComputerUsers
{
    public class ComputerUserShort
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string UserId { get; set; }
        public bool ShowOnExtPageDepartmentCases { get; set; }
    }
}
