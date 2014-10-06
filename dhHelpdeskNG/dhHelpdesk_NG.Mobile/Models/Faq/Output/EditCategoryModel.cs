namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;

    public sealed class EditCategoryModel
    {
        public EditCategoryModel(int id, string name, bool hasFaqs, bool hasSubcategories)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("id", "Must be more than zero.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }

            this.Id = id;
            this.Name = name;
            this.HasFaqs = hasFaqs;
            this.HasSubcategories = hasSubcategories;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public bool HasFaqs { get; private set; }

        public bool HasSubcategories { get; private set; }
    }
}