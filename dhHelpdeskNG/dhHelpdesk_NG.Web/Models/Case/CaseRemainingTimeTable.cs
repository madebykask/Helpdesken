namespace DH.Helpdesk.Web.Models.Case
{
    public sealed class CaseRemainingTimeTable
    {
        public CaseRemainingTimeTable()
        {

        }

        public CaseRemainingTimeTable(
            int remaningTime,
            int? remaningTimeUntil, 
            int  maxRemaningTime,
            bool isHour)
        {
            this.RemaningTime = remaningTime;
            this.RemaningTimeUntil = remaningTimeUntil;
            this.MaxRemaningTime = maxRemaningTime;
            this.IsHour = isHour;            
        }

        public int RemaningTime { get; private set; }

        public int? RemaningTimeUntil { get; private set; }

        public int MaxRemaningTime { get; private set; }

        public bool IsHour { get; private set; }
    }
}