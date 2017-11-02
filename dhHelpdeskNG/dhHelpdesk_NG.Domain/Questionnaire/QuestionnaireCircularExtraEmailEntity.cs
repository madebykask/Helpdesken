namespace DH.Helpdesk.Domain.Questionnaire
{
    public class QuestionnaireCircularExtraEmailEntity : Entity
    {
        public int QuestionnaireCircular_Id { get; set; }
        public string Email { get; set; }

        public virtual QuestionnaireCircularEntity QuestionnaireCircular { get; set; }
    }
}
