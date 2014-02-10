namespace DH.Helpdesk.Domain.Servers
{
    using global::System;

    public class ServerLogicalDrive : Entity
    {
        public double FreeBytes { get; set; }
        public double TotalBytes { get; set; }
        public int DriveType { get; set; }
        public int Server_Id { get; set; }
        public string DriveLetter { get; set; }
        public string FileSystemName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Server Server { get; set; }
    }
}
