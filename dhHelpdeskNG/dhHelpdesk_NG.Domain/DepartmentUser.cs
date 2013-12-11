using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class DepartmentUser
    {
        public int Department_Id { get; set; }
        public int User_Id { get; set; }
    }
}
