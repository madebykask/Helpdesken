namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IFaqRepository : IRepository<FAQ>
    {
        bool AnyFaqWithCategoryId(int categoryId);

        void Update(ExistingFaqDto existingFaq);

        List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId);

        List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId);
            
        void DeleteById(int faqId);

        Faq FindById(int faqId);

        void Add(NewFaqDto newFaq);

        List<FaqOverview> FindOverviewsByCategoryId(int categoryId);

        List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId);
    }
}