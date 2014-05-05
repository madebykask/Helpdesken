namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsModel
    {
        public StateFieldsModel(
            int idState,
            ConfigurableFieldModel<bool> isStolen,
            ConfigurableFieldModel<string> replaced,
            ConfigurableFieldModel<bool> isSendBack,
            ConfigurableFieldModel<DateTime?> scrapDate)
        {
            this.StateId = idState;
            this.IsStolen = isStolen;
            this.Replaced = replaced;
            this.IsSendBack = isSendBack;
            this.ScrapDate = scrapDate;
        }

        public int StateId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> IsStolen { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Replaced { get; set; }

        [NotNull]
        public ConfigurableFieldModel<bool> IsSendBack { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> ScrapDate { get; set; }
    }
}