namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Shared
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