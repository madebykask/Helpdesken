namespace DH.Helpdesk.Web.Models.Common
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GridColumnHeaderModel
    {
        public GridColumnHeaderModel(string name, string caption)
        {
            this.Name = name;
            this.Caption = caption;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}