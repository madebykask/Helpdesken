using System.Web;

namespace DH.Helpdesk.EForm.Model.Entities
{
    public class FormField
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string FormFieldName { get; set; }
        public string FormFieldValue { get; set; }
        public string InitialFormFieldValue { get; set; }
        public bool HCMData { get; set; }
        public bool ParentGVFields { get; set; }

        public string GVLabel { get; set; }
        public string GVValue { get; set; }
        public bool GVShow { get; set; }

        
        //public int SortOrder { get; set; }
        //public string Label { get; set; }
        //public string GroupKey { get; set; }
    }
}