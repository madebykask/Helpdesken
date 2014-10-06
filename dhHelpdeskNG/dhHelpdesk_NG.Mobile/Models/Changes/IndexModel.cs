namespace DH.Helpdesk.Mobile.Models.Changes
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class IndexModel
    {
        public IndexModel(string activeTab)
        {
            this.ActiveTab = activeTab;
        }

        [NotNullAndEmpty]
        public string ActiveTab { get; set; }
    }
}