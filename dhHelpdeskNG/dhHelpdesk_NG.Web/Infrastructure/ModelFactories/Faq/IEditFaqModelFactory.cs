namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Web.Models.Faq.Output;

    public interface IEditFaqModelFactory
    {
        EditFaqModel Create(Faq faq, List<CategoryWithSubcategories> categories, List<string> fileNames, List<ItemOverview> workingGroups);
    }
}