namespace DH.Helpdesk.Domain
{
    using global::System;

    public class CaseSettings : Entity
    {
        public int Customer_Id { get; set; }
        public int ColOrder { get; set; }
        public int Line { get; set; }
        public int MinWidth { get; set; }
        public int? User_Id { get; set; }
        public int UserGroup { get; set; }
        public string Name { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
