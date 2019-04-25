using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public interface IFaqRepository : IRepository<FaqEntity>
    {
        bool AnyFaqWithCategoryId(int categoryId);

        void Add(NewFaq newFaq);

        void UpdateSwedishFaq(ExistingFaq faq);

        void UpdateOtherLanguageFaq(ExistingFaq faq);

        List<FaqEntity> GetFaqsByCustomerId(int customerId, bool includePublic = true);

        void UpdateDate(int id, DateTime changedDate);
    }
}