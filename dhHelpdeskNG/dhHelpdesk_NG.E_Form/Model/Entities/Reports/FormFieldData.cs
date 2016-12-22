using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.EForm.Model.Entities.Reports
{
    public class FormFieldData
    {
        public int CaseId { get; set; }
        public DateTime RegTime { get; set; }
        public string CaseNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string FormFieldName { get; set; }
        public string FormFieldValue { get; set; }        
    }
}
