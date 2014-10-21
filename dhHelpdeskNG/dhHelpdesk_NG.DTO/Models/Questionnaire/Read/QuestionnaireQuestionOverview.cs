namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireQuestionOverview
    {
        public QuestionnaireQuestionOverview(
            int id,
            string question,
            string number,
            bool showNote,
            string noteText,
            List<QuestionnaireQuestionOptionOverview> oprions)
        {
            this.Id = id;
            this.Question = question;
            this.Number = number;
            this.ShowNote = showNote;
            this.NoteText = noteText;
            this.Oprions = oprions;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Question { get; private set; }

        public string Number { get; private set; }

        public bool ShowNote { get; private set; }

        public string NoteText { get; private set; }

        public List<QuestionnaireQuestionOptionOverview> Oprions { get; private set; }
    }
}