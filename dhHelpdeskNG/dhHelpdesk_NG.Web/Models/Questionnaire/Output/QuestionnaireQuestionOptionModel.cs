namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class QuestionnaireQuestionOptionModel
    {
        public QuestionnaireQuestionOptionModel(int id, string option, int position)
        {
            this.Id = id;
            this.Option = option;
            this.Position = position;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Option { get; private set; }

        public int Position { get; private set; }
    }
}