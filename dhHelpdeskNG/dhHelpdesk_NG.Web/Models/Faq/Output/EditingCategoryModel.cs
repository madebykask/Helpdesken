namespace dhHelpdesk_NG.Web.Models.Faq.Output
{
    public sealed class EditingCategoryModel
    {
        public EditingCategoryModel(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}