namespace DH.Helpdesk.Web.Common.Models.Case
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
            RemaningTime = remaningTime;
            RemaningTimeUntil = remaningTimeUntil;
            MaxRemaningTime = maxRemaningTime;
            IsHour = isHour;            
        }

        public int RemaningTime { get; private set; }

        public int? RemaningTimeUntil { get; private set; }

        public int MaxRemaningTime { get; private set; }

        public bool IsHour { get; private set; }
    }
}