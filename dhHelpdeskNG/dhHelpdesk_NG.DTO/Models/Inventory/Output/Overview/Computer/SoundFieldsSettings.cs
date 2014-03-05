namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Computer
{
    public class SoundFieldsSettings
    {
        public SoundFieldsSettings(string soundCard)
        {
            this.SoundCard = soundCard;
        }

        public string SoundCard { get; set; }
    }
}