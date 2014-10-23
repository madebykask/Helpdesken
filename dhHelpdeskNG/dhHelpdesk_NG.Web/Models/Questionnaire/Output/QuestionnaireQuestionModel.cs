namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class QuestionnaireQuestionModel
    {
        public QuestionnaireQuestionModel(
            int id,
            string question,
            string number,
            bool isShowNote,
            string noteText,
            List<QuestionnaireQuestionOptionModel> oprions)
        {
            this.Id = id;
            this.Question = question;
            this.Number = number;
            this.IsShowNote = isShowNote;
            this.NoteText = noteText;
            this.Options = oprions;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Question { get; private set; }

        public string Number { get; private set; }

        public bool IsShowNote { get; private set; }

        public string NoteText { get; private set; }

        [NotNull]
        public List<QuestionnaireQuestionOptionModel> Options { get; private set; }

        [LocalizedRequired]
        public int? SelectedOptionId { get; set; }
    }
}