namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Computers;

    using global::System;

    public class LogicalDrive : Entity
    {
        public double FreeBytes { get; set; }
        public double TotalBytes { get; set; }
        public int Computer_Id { get; set; }
        public int DriveType { get; set; }
        public string DriveLetter { get; set; }
        public string FileSystemName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Computer Computer { get; set; }
    }
}
