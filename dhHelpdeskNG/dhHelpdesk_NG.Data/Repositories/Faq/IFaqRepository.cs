namespace dhHelpdesk_NG.Data.Repositories.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Input;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

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