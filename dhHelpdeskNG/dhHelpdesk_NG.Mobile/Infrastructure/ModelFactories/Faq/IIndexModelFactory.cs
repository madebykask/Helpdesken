namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Mobile.Models.Faq.Output;

    public interface IIndexModelFactory
    {
        IndexModel Create(List<CategoryWithSubcategories> categories, int? selectedCategoryId, List<FaqOverview> firstCategoryFaqs);
    }
}