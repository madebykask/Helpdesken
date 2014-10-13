namespace DH.Helpdesk.Web.Areas.Licenses.Models.Common
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class BaseEditModel
    {
        public abstract EntityModelType Type { get; }

        [MinValue(0)]
        [HiddenInput]
        public int Id { get; set; }

        public bool IsNew()
        {
            return this.Id == 0;
        }
    }
}