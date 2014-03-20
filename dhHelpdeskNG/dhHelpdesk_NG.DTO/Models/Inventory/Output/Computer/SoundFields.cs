namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class SoundFields
    {
        public SoundFields(string soundCard)
        {
            this.SoundCard = soundCard;
        }

        public string SoundCard { get; set; }
    }
}