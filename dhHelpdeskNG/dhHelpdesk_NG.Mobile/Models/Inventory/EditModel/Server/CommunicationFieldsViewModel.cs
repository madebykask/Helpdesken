namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Server
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsViewModel
    {
        public CommunicationFieldsViewModel()
        {
        }

        public CommunicationFieldsViewModel(
            CommunicationFieldsModel communicationFieldsModel,
            SelectList networkAdapters)
        {
            this.CommunicationFieldsModel = communicationFieldsModel;
            this.NetworkAdapters = networkAdapters;
        }

        [NotNull]
        public CommunicationFieldsModel CommunicationFieldsModel { get; set; }

        [NotNull]
        public SelectList NetworkAdapters { get; set; }
    }
}