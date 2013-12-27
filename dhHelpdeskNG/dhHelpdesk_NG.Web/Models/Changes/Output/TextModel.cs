namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class TextModel
    {
        [Email]
        public string Name { get; set; }
    }
}