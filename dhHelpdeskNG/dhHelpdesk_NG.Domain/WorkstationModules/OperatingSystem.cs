namespace DH.Helpdesk.Domain.WorkstationModules
{
    using global::System;

    public class OperatingSystem : Entity
    {
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
