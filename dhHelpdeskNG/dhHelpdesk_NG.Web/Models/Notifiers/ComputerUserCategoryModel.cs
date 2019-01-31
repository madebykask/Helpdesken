using DH.Helpdesk.Domain.Computers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.Notifiers
{
    public class ComputerUserCategoryModel
    {
        #region ctor()

        public ComputerUserCategoryModel(IList<SelectListItem> categories)
        {
            ComputerUserCategories = categories;
        }

        public ComputerUserCategoryModel(IList<SelectListItem> categories, ComputerUserCategory category, string extendedCaseUrl)
            : this(categories)
        {
            if (category != null)
            {
                ComputerUserCategoryID = category.ID;
                Name = category.Name;
                IsReadOnly = category.IsReadOnly;
                ExtendedCaseFormID = category.ExtendedCaseFormID;
                ExtendedCasePath = extendedCaseUrl;
            }
        }

        #endregion

        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public int? ExtendedCaseFormID { get; set; }
        public string ExtendedCasePath { get; set; }
        public int? ComputerUserCategoryID { get; set; }
        
        public IList<SelectListItem> ComputerUserCategories { get; set; }
    }
}