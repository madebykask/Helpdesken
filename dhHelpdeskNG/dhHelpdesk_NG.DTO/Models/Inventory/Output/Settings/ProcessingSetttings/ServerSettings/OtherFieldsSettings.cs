namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(ProcessingFieldSetting infoFieldSetting, ProcessingFieldSetting otherFieldSetting, ProcessingFieldSetting urlFieldSetting, ProcessingFieldSetting url2FieldSetting, ProcessingFieldSetting ownerFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
            this.OtherFieldSetting = otherFieldSetting;
            this.URLFieldSetting = urlFieldSetting;
            this.URL2FieldSetting = url2FieldSetting;
            this.OwnerFieldSetting = ownerFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting InfoFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting OtherFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting URLFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting URL2FieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting OwnerFieldSetting { get; set; }
    }
}