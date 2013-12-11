using System;

namespace dhHelpdesk_NG.Domain
{
    public class FormFieldValue : Entity
    {
        public int Case_Id { get; set; }
        public int FormField_Id { get; set; }        
        public string FormFieldValues { get; set; }
    }
}
