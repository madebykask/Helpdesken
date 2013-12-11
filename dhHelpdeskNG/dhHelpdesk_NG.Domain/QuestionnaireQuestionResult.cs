using System;

namespace dhHelpdesk_NG.Domain
{
    public class QuestionnaireQuestionResult : Entity
    {
        public int QuestionnaireQuestionOption_Id { get; set; }
        public int QuestionnaireResult_Id { get; set; }
        public string QuestionnaireQuestionNote { get; set; }

        public virtual QuestionnaireQuestionOption QuestionnaireQuestionOption { get; set; }
        public virtual QuestionnaireResult QuestionnaireResult { get; set; }
    }
}
