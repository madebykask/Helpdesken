namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class IndexModel
    {
        public IndexModel(TreeContent categories, List<FaqOverviewModel> firstCategoryFaqs, bool userHasFaqAdminPermission, bool showDetails = false)
        {
            this.UserHasFaqAdminPermission = userHasFaqAdminPermission;
            if (categories == null)
            {
                throw new ArgumentNullException("categories", "Value cannot be null.");
            }

            if (firstCategoryFaqs == null)
            {
                throw new ArgumentNullException("firstCategoryFaqs", "Value cannot be null or empty.");
            }

            this.Categories = categories;
            this.FirstCategoryFaqs = firstCategoryFaqs;
            this.ShowDetails = showDetails;
        }

        public List<FaqOverviewModel> FirstCategoryFaqs { get; private set; }

        public TreeContent Categories { get; private set; }

        public bool UserHasFaqAdminPermission { get; private set; }

        public bool ShowDetails { get; set; }
    }
}