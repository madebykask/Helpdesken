namespace dhHelpdesk_NG.DTO.DTOs.Faq.Input
{
    using System;

    public sealed class NewCategoryDto : INewEntity
    {
        public NewCategoryDto(string name, DateTime createdDate, int customerId, int? parentCategoryId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException("customerId", "Value cannot be null or empty.");
            }

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

        public string Name { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int CustomerId { get; private set; }

        public int? ParentCategoryId { get; private set; }

        public int Id { get; set; }
    }
}
