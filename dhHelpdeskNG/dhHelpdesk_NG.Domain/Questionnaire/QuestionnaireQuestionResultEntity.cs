namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireQuestionResultEntity : Entity
    {
        public int QuestionnaireQuestionOptionId { get; set; }
        public int QuestionnaireResultId { get; set; }
        public string QuestionnaireQuestionNote { get; set; }

        public virtual QuestionnaireQuestionOptionEntity QuestionnaireQuestionOption { get; set; }
        public virtual QuestionnaireResultEntity QuestionnaireResult { get; set; }
    }
}
