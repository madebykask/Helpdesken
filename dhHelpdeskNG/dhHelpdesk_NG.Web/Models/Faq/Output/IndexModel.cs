using System.Linq;
using DH.Helpdesk.BusinessData.Enums;

namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class IndexModel
    {
        public IndexModel(TreeContent categories, List<FaqOverviewModel> firstCategoryFaqs, int languageId, bool userHasFaqAdminPermission, bool showDetails = false)
        {
            UserHasFaqAdminPermission = userHasFaqAdminPermission;
            if (categories == null)
                throw new ArgumentNullException(nameof(categories), "Value cannot be null.");
            

            if (firstCategoryFaqs == null)
                throw new ArgumentNullException(nameof(firstCategoryFaqs), "Value cannot be null or empty.");
            
            Categories = categories;
            FirstCategoryFaqs = firstCategoryFaqs;
            LanguageId = languageId;
            ShowDetails = showDetails;
        }

        public List<FaqOverviewModel> FirstCategoryFaqs { get; private set; }

        public TreeContent Categories { get; private set; }

        public bool UserHasFaqAdminPermission { get; private set; }

        public bool ShowDetails { get; private set; }

        public int LanguageId { get; private set; }

        public string SortBy { get; set; }

        public SortOrder SortOrder { get; set; }

        #region Methods

        public string GetFirstCategoryId()
        {
            var category = Categories?.Items.FirstOrDefault();
            return category?.Value ?? "";
        }

        #endregion
    }
}