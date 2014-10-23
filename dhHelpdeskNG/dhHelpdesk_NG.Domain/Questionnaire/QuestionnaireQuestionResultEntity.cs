namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireQuestionResultEntity : Entity
    {
        #region Public Properties

        public string QuestionnaireQuestionNote { get; set; }

        public virtual QuestionnaireQuestionOptionEntity QuestionnaireQuestionOption { get; set; }

        public int QuestionnaireQuestionOption_Id { get; set; }

        public virtual QuestionnaireResultEntity QuestionnaireResult { get; set; }

        public int QuestionnaireResult_Id { get; set; }

        #endregion
    }
}