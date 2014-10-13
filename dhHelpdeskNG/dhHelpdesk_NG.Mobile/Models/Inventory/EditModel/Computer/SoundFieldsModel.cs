namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoundFieldsModel
    {
        public SoundFieldsModel()
        {
        }

        public SoundFieldsModel(ConfigurableFieldModel<string> soundCard)
        {
            this.SoundCard = soundCard;
        }

        [NotNull]
        public ConfigurableFieldModel<string> SoundCard { get; set; }
    }
}