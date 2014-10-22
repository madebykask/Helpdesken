namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public interface IFaqRepository : IRepository<FaqEntity>
    {
        bool AnyFaqWithCategoryId(int categoryId);

        void Update(ExistingFaq existingFaq);

        List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId);

        List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId);
            
        void DeleteById(int faqId);

        Faq FindById(int faqId);

        void Add(NewFaq newFaq);

        List<FaqOverview> FindOverviewsByCategoryId(int categoryId);

        List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId);
    }
}