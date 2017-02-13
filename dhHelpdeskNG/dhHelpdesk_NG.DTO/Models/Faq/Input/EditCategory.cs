using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.BusinessData.Models.Faq.Input
{
    public sealed class EditCategory : INewBusinessModel
    {
        public EditCategory(int id, string name, int languageId, DateTime changedDate)
        {
            Id = id;
            Name = name;
            LanguageId = languageId;
            ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public int LanguageId { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}
