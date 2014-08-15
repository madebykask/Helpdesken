namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    public class SoundFields
    {
        public SoundFields(string soundCard)
        {
            this.SoundCard = soundCard;
        }

        public string SoundCard { get; set; }

        public static SoundFields CreateDefault()
        {
            return new SoundFields(null);
        }
    }
}