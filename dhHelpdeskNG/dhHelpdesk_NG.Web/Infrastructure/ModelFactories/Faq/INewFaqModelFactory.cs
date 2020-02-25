namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Models.Faq.Output;

    public interface INewFaqModelFactory
    {
        NewFaqModel Create(
            string temporaryId, 
            List<CategoryWithSubcategories> categories, 
            int categoryId, 
            List<ItemOverview> workingGroups,
            bool userHasFaqAdminPermission,
			List<string> fileUploadWhiteList);
    }
}