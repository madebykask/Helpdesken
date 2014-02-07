namespace DH.Helpdesk.Web.Models.Problem
{
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public class LogEditViewModel
    {
        public DropDownWithSubmenusContent FinishingCauses { get; set; }

        public LogEditModel Log { get; set; }
    }
}