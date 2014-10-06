namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Mobile.Models.Faq.Output;

    public interface IEditFaqModelFactory
    {
        EditFaqModel Create(Faq faq, List<CategoryWithSubcategories> categories, List<string> fileNames, List<ItemOverview> workingGroups);
    }
}