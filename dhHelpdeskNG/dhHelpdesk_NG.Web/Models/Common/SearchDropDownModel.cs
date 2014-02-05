namespace dhHelpdesk_NG.Web.Models.Common
{
    using System.Web.Mvc;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class SearchDropDownModel<TList> where TList : MultiSelectList
    {
        public SearchDropDownModel(bool show)
        {
            this.Show = show;
        }

        public SearchDropDownModel(bool show, string caption, TList list)
        {
            this.Show = show;
            this.Caption = caption;
            this.List = list;
        }

        public bool Show { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public TList List { get; set; }
    }
}