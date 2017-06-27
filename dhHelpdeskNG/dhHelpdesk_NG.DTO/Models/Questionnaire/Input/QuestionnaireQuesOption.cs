
namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class QuestionnaireQuesOption : INewBusinessModel    
    {
        public QuestionnaireQuesOption()
        {
            
        }
        public QuestionnaireQuesOption(int id, int questionId, int optionPos, string option, int optionValue, int languageId, DateTime changedDate)
        {
            this.Id = id;
            this.QuestionId = questionId;
            this.OptionPos = optionPos;
            this.Option = option;
            this.OptionValue = optionValue;
            this.LanguageId = languageId;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }
        
        public int QuestionId { get; set; }

        [MinValue(0)]
        public int OptionPos { get;  set; }

        [NotNullAndEmpty]
        public string Option { get;  set; }

        [MinValue(0)]
        public int OptionValue { get;  set; }
       
        public int LanguageId { get;  set; }

		[MaxLength(200)]
		public string IconId { get; set; }

        public DateTime ChangedDate { get;  set; }

        public byte[] IconSrc { get; set; }
    }
}