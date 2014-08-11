namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsViewModel
    {
        public CommunicationFieldsViewModel()
        {
        }

        public CommunicationFieldsViewModel(CommunicationFieldsModel communicationFieldsModel, ConfigurableFieldModel<SelectList> networkAdapters)
        {
            this.CommunicationFieldsModel = communicationFieldsModel;
            this.NetworkAdapters = networkAdapters;
        }

        [NotNull]
        public CommunicationFieldsModel CommunicationFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> NetworkAdapters { get; set; }
    }
}