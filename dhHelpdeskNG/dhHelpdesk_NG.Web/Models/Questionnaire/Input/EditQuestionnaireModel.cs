using DH.Helpdesk.Web.Models.Questionnaire.Output;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class EditQuestionnaireModel
    {
        public EditQuestionnaireModel()
        {

        }

        public EditQuestionnaireModel(int id, string name, string description, int languageId, DateTime createDate, SelectList languages, 
               List<QuestionnaireQuestionsOverviewModel> questionList)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.LanguageId = languageId;
            this.CreateDate = createDate;
            this.Languages = languages;
            this.Questions = questionList;
        }

        [IsId]
        public int Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(100)]
        [LocalizedDisplay("Name")]
        public string Name { get; set; }

        [LocalizedDisplay("Description")]
        public string Description { get; set; }

        [LocalizedDisplay("CustomerId")]
        public int CustomerId { get; set; }

        [LocalizedDisplay("LanquageId")]
        public int LanguageId { get; set; }

        [LocalizedDisplay("CreateDate")]
        public DateTime CreateDate { get; set; }

        public SelectList Languages { get; set; }

        public bool IsSent { get; set; }

        public List<QuestionnaireQuestionsOverviewModel> Questions { get; set; } 
    }
}