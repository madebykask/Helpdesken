namespace dhHelpdesk_NG.Web.Models.Faq.Output
{
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class EditingCategoryModel
    {
        public EditingCategoryModel(string name)
        {
            this.Name = name;
        }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Name { get; private set; }
    }
}