using System.Web.Mvc;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Models.Faq.Output;

    public interface IEditFaqModelFactory
    {
        EditFaqModel Create(
            Faq faq, 
            List<CategoryWithSubcategories> categories, 
            List<string> fileNames, 
            List<ItemOverview> workingGroups,
            bool userHasFaqAdminPermission,
            SelectList languages,
            int languageId,
            bool showDetails,
			List<string> fileUploadWhiteList);
    }
}