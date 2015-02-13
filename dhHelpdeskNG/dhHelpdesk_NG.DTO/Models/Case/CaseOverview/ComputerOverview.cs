namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    public sealed class ComputerOverview
    {
        public ComputerOverview(
                string number, 
                string computerType, 
                string place)
        {
            this.Place = place;
            this.ComputerType = computerType;
            this.PcNumber = number;
        }

        public string PcNumber { get; private set; }

        public string ComputerType { get; private set; }

        public string Place { get; private set; }
    }
}