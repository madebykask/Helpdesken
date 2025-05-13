namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class EditQuestionnaire : INewBusinessModel
    {
        public EditQuestionnaire(int id, string name, string description, int languageId, DateTime changedDate)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.LanguageId = languageId;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }
        
        [NotNullAndEmpty]
        public string Name { get; private set; }

        public string Description { get; private set; }
        
        public int LanguageId { get; private set; }
		public string Identifier { get; set; }
		public bool ExcludeAdministrators { get; set; }
        public bool UseBase64Images { get; set; }


        public DateTime ChangedDate { get; private set; }
    }
}