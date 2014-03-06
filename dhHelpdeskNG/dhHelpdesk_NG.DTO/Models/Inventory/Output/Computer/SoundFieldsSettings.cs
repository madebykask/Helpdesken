namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
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