namespace DH.Helpdesk.Web.Models.Faq.Output
{
    public sealed class NewCategoryModel
    {
        public NewCategoryModel(int? parentCategoryId, bool userHasFaqAdminPermission)
        {
            this.UserHasFaqAdminPermission = userHasFaqAdminPermission;
            this.ParentCategoryId = parentCategoryId;
        }

        public int? ParentCategoryId { get; private set; }

        public bool UserHasFaqAdminPermission { get; private set; }
    }
}