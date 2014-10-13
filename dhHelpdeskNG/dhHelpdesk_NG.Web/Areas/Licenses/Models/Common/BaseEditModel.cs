namespace DH.Helpdesk.Web.Areas.Licenses.Models.Common
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class BaseEditModel
    {
        [MinValue(0)]
        [HiddenInput]
        public int Id { get; set; }

        public abstract string Title { get; }
    }
}