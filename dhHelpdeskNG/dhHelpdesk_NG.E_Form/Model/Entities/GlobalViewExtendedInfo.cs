using System;

namespace DH.Helpdesk.EForm.Model.Entities
{
    public class GlobalViewExtendedInfo
    {
        public int Id { get; set; }
        public string FormFieldName { get; set; }
        public string FormFieldIdentifier { get; set; }
        public string FormFieldValue { get; set; }
        public string FormFieldCode { get; set; }
        public int FormFieldId { get; set; }
        
        public int CustomerId { get; set; }
    }
}
