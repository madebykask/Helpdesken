namespace dhHelpdesk_NG.Web.Models.Problem
{
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public class LogEditViewModel
    {
        public DropDownWithSubmenusContent FinishingCauses { get; set; }

        public LogEditModel Log { get; set; }
    }
}