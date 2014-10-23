namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public interface IFaqRepository : IRepository<FaqEntity>
    {
        bool AnyFaqWithCategoryId(int categoryId);

        void Update(ExistingFaq existingFaq);
            
        void Add(NewFaq newFaq);
    }
}