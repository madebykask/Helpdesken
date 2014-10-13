namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsViewModel
    {
        public MemoryFieldsViewModel()
        {
        }

        public MemoryFieldsViewModel(MemoryFieldsModel memoryFieldsModel, SelectList raMs)
        {
            this.MemoryFieldsModel = memoryFieldsModel;
            this.RAMs = raMs;
        }

        [NotNull]
        public MemoryFieldsModel MemoryFieldsModel { get; set; }
        
        [NotNull]
        public SelectList RAMs { get; set; }
    }
}