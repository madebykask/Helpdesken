namespace DH.Helpdesk.Services.Response.Questionnaire
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OptionResult
    {
        public OptionResult(int optionId, int count)
        {
            this.OptionId = optionId;
            this.Count = count;
        }

        [IsId]
        public int OptionId { get; private set; }

        [MinValue(0)]
        public int Count { get; private set; }
    }
}
