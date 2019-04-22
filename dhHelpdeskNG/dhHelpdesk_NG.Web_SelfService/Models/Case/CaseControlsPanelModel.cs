namespace DH.Helpdesk.SelfService.Models.Case
{
    public class CaseControlsPanelModel
    {
        public CaseControlsPanelModel(int position, bool isExtendedCase)
        {
            IsExtendedCase = isExtendedCase;
            Position = position;
            CanSave = true;
        }

        public int Position { get; }
        public bool IsExtendedCase { get; }
        public bool CanSave { get; set; }
    }
}