namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    public class SoundFieldsModel
    {
        public SoundFieldsModel()
        {
        }

        public SoundFieldsModel(ConfigurableFieldModel<string> soundCard)
        {
            this.SoundCard = soundCard;
        }

        public ConfigurableFieldModel<string> SoundCard { get; set; }
    }
}