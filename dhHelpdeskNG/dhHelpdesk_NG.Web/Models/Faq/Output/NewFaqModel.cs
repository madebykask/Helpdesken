namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using System;

    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class NewFaqModel
    {
        public NewFaqModel(string temporaryId, DropDownWithSubmenusContent categories, DropDownContent workingGroups)
        {
            if (string.IsNullOrEmpty(temporaryId))
            {
                throw new ArgumentNullException("temporaryId", "Value cannot be null or empty.");
            }

            if (categories == null)
            {
                throw new ArgumentNullException("categories", "Value cannot be null.");
            }

            if (workingGroups == null)
            {
                throw new ArgumentNullException("workingGroups", "Value cannot be null.");
            }

            this.Categories = categories;
            this.WorkingGroups = workingGroups;
            this.TemporaryId = temporaryId;
        }

        public string TemporaryId { get; private set; }

        public DropDownContent WorkingGroups { get; private set; }

        public DropDownWithSubmenusContent Categories { get; private set; }
    }
}