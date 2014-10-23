namespace DH.Helpdesk.Web.Models.Questionnaire.Output
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class QuestionInputModel
    {
        public int Id { get; set; }

        public string NoteText { get; set; }

        [LocalizedRequired]
        public int? SelectedOptionId { get; set; }
    }
}