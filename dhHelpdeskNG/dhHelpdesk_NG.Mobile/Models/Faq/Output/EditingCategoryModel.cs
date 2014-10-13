namespace DH.Helpdesk.Mobile.Models.Faq.Output
{
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

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