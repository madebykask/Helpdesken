
namespace DH.Helpdesk.BusinessData.Models
{
    public class CaseFieldSettingsWithLanguage
    {
        public int Id { get; set; }
        public int Language_Id { get; set; }
        public string FieldHelp { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string EMailIdentifier { get; set; }
    }

    public class CaseFieldSettingsForTranslation
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public int Customer_Id { get; set; }
        public int Language_Id { get; set; }
    }

    public class CustomCaseFieldSettingsWithLanguage : CaseFieldSettingsWithLanguage
    {        
        public int? CustomerId { get; set; }
    }


}
