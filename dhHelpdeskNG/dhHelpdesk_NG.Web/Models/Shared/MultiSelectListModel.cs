namespace DH.Helpdesk.Web.Models.Shared
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class MultiSelectListModel
    {
        public MultiSelectListModel(
                string headerText,
                SelectListItem[] availableItems,
                SelectListItem[] selectedItems)
        {
            this.HeaderText = headerText;
            this.AvailableItems = availableItems;
            this.SelectedItems = selectedItems;
        }

        public string HeaderText { get; private set; } 
        
        [NotNull]
        public SelectListItem[] AvailableItems { get; set; }

        [NotNull]
        public SelectListItem[] SelectedItems { get; set; }
    }
}