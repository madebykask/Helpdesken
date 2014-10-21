namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Read
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireQuestionOptionOverview
    {
        public QuestionnaireQuestionOptionOverview(int id, string option, int value, int position)
        {
            this.Id = id;
            this.Option = option;
            this.Value = value;
            this.Position = position;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Option { get; private set; }

        public int Value { get; private set; }

        public int Position { get; private set; }
    }
}