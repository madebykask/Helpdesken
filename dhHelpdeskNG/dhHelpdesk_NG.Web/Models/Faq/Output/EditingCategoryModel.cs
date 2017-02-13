using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Models.Faq.Output
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class EditingCategoryModel
    {
        public EditingCategoryModel(int id, string name, SelectList languages)
        {
            this.Id = id;
            this.Name = name;
            this.Languages = languages;
        }

        public int Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Name { get; private set; }

        [LocalizedDisplay("LanquageId")]
        public int LanguageId { get; set; }

        public SelectList Languages { get; set; }

        public bool IsNew { get { return Id <= 0; } }
    }
}