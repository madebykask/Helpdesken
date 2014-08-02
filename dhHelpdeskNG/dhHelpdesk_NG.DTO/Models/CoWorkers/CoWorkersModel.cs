using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.BusinessData.Models.CoWorkers
{                  
    
    public class CoWorker
    {
        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string JobKey { get; set; }

        public string Email { get; set; }

    }
    
}
