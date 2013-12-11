using System;

namespace dhHelpdesk_NG.Domain
{
    public class FormField : Entity
    {
        public int Form_Id { get; set; }
        public int FormFieldType { get; set; }
        public int PageNumber { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string FormFieldName { get; set; }

        public virtual Form Form { get; set; }
    }
}
