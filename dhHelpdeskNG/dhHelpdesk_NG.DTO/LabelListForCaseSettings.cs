using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.DTO
{
    public class LabelListForCaseSettings
    {
        public int Id { get; set; }
        public int UserGroup_Id { get; set; }
        public int Customer_Id { get; set; }
        public int CaseFieldSettings_Id { get; set; }
        public int LanguageCaseFieldSetting_Id { get; set; }
        //public string CaseFieldName { get; set; }
        public string CaseSettingName { get; set; }
        public string Label { get; set; }
    }
}
