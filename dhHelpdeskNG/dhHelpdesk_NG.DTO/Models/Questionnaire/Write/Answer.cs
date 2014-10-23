namespace DH.Helpdesk.BusinessData.Models.Questionnaire.Write
{
    public class Answer
    {
        public Answer(string note, int optionId)
        {
            this.Note = note;
            this.OptionId = optionId;
        }

        public string Note { get; private set; }

        public int OptionId { get; private set; }
    }
}