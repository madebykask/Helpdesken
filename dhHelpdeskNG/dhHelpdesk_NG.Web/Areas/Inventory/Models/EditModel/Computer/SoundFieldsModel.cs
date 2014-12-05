namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
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