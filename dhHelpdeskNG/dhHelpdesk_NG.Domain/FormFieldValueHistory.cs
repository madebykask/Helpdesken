using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain
{
    public class FormFieldValueHistory : Entity
    {
        public int Case_Id { get; set; }
        public int FormField_Id { get; set; }
        public int CaseHistory_Id { get; set; }
        public string FormFieldValue { get; set; }
        //public string InitialFormFieldValue { get; set; }
        public string FormFieldText { get; set; }
        public string InitialFormFieldText { get; set; }
    }
}
