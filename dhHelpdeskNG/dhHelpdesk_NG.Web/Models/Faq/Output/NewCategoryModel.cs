namespace DH.Helpdesk.Web.Models.Faq.Output
{
    public sealed class NewCategoryModel
    {
        public NewCategoryModel(int? parentCategoryId)
        {
            this.ParentCategoryId = parentCategoryId;
        }

        public int? ParentCategoryId { get; private set; }
    }
}