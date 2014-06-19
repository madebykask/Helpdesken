namespace DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange
{
    public sealed class UpdatedLogFields
    {
        public UpdatedLogFields(string logNote)
        {
            this.LogNote = logNote;
        }

        public string LogNote { get; private set; }
    }
}