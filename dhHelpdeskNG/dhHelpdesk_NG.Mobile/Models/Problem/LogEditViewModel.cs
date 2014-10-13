namespace DH.Helpdesk.Mobile.Models.Problem
{
    using DH.Helpdesk.Mobile.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public class LogEditViewModel
    {
        public DropDownWithSubmenusContent FinishingCauses { get; set; }

        public LogEditModel Log { get; set; }
    }
}