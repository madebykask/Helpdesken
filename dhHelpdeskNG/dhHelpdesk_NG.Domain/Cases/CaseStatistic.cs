namespace DH.Helpdesk.Domain.Cases
{
    public class CaseStatistic : Entity
    {
        public int CaseId { get; set; }

        public int? WasSolvedInTime { get; set; }
    }
}
