namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class SoundFields
    {
        public SoundFields(ConfigurableFieldModel<string> soundCard)
        {
            this.SoundCard = soundCard;
        }

        public ConfigurableFieldModel<string> SoundCard { get; set; }
    }
}