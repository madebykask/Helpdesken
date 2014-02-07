
namespace DH.Helpdesk.BusinessData.Models
{
    public class ListCases
    {
        public int? CFS_Id { get; set; }
        public int? CFSL_CaseFieldSettings_Id { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
    }

    public class CaseListToCase
    {
        public int CFS_Id { get; set; }
        public int? Customer_Id { get; set; }
        public int FieldSize { get; set; }
        public int Language_Id { get; set; }
        public int Required { get; set; }
        public int ShowExternal { get; set; }
        public int ShowOnStartPage { get; set; }
        public string LabelNotToChange { get; set; }
        public string LabelToChange { get; set; }
    }

    public class CaseSettingList
    {
        public string Name { get; set; }
    }
}
