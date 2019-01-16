namespace DH.Helpdesk.Domain.Computers
{
    public class WorkstationTabSettingLanguage
    {
        public int WorkstationTabSetting_Id { get; set; }
        public int Language_Id { get; set; }
        public string Label { get; set; }
        public string FieldHelp { get; set; }

        public virtual WorkstationTabSetting WorkstationTabSetting { get; set; }
        public virtual Language Language { get; set; }
    }
}