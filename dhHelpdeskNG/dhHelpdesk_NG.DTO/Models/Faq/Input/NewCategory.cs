namespace DH.Helpdesk.BusinessData.Models.Faq.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewCategory : INewBusinessModel
    {
        public NewCategory(string name, DateTime createdDate, int customerId, int? parentCategoryId)
        {
            if (parentCategoryId.HasValue)
            {
                if (parentCategoryId <= 0)
                {
                    throw new ArgumentOutOfRangeException("parentCategoryId", "Must be more than zero.");
                }
            }

            this.Name = name;
            this.CreatedDate = createdDate;
            this.CustomerId = customerId;
            this.ParentCategoryId = parentCategoryId;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        public int? ParentCategoryId { get; private set; }

        [IsId]
        public int Id { get; set; }
    }
}
