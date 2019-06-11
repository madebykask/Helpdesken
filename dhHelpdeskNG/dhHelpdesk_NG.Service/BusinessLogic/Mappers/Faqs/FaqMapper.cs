namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Faqs
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Domain.Faq;

    public static class FaqMapper
    {
        public static FaqInfoOverview[] MapToOverviews(this IQueryable<FaqEntity> query)
        {
            var entities = query.Select(f => new
                                        {
                                            f.Id,
                                            f.ChangedDate,
                                            f.CreatedDate,
                                            f.FAQCategory,
                                            f.Answer,
                                            f.FAQQuery,
                                            f.FAQCategory.ParentFAQCategory,
                                            f.Customer
                                        }).ToArray();

            return entities.Select(f => new FaqInfoOverview
                                            {
                                                Id = f.Id,
                                                CreatedDate = f.CreatedDate,
                                                ChangeDate = f.ChangedDate,
                                                Category = f.FAQCategory,
                                                Answer = f.Answer,
                                                Text = f.FAQQuery,
                                                CustomerName = f.Customer.Name
                                            }).ToArray();
        }
    }
}