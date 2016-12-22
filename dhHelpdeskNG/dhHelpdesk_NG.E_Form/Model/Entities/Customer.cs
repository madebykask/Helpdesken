namespace ECT.Model.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public int WorkingDayStart { get; set; }
        public int WorkingDayEnd { get; set; }
        public int Setting_CalcSolvedInTimeByLatestSLADate { get; set; }
        public int Setting_TimeZone_offset { get; set; }
    }
}
