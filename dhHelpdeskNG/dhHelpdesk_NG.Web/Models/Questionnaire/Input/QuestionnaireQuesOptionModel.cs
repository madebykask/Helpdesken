using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class QuestionnaireQuesOptionModel
    {
        public QuestionnaireQuesOptionModel()
        {

        }

        public QuestionnaireQuesOptionModel(int id, int questionId, int optionPos, string option, int optionValue, int languageId, DateTime changedDate)
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

        [LocalizedDisplay("QuestionId")]
        public int QuestionId { get; set; }

        [Required]
        [LocalizedDisplay("OptionPos")]        
        public int OptionPos { get; set; }

        [Required]
        [StringLength(100)]
        [LocalizedDisplay("Option")]
        public string Option { get; set; }

        [Required]
        [LocalizedDisplay("OptionValue")]
        public int OptionValue { get; set; }
        
        [LocalizedDisplay("LanguageId")]        
        public int LanguageId { get; set; }

		[LocalizedDisplay("IconId")]
		public string IconId { get; set; }

		[LocalizedDisplay("ChangedDate")]
        public DateTime ChangedDate { get; set; }

        public string IconSrc { get; set; }
    }
}