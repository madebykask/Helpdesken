namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSolutionSettingsField : Entity
    {
        public string CaseSolutionConditionId { get; set; }

        public int CaseSolutionId { get; set; }

        public string PropertyName { get; set; }

        public string Text { get; set; }

        public string[] SelectedValues { get; set; }

    }
}
    