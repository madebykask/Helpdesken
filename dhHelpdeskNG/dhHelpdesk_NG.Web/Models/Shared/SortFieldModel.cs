namespace DH.Helpdesk.Web.Models.Shared
{
    using DH.Helpdesk.Common.Enums;

    public sealed class SortFieldModel
    {
        public string Name { get; set; }

        public SortBy? SortBy { get; set; }
    }
}