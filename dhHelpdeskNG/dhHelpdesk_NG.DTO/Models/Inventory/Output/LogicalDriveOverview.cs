namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class LogicalDriveOverview
    {
        public LogicalDriveOverview(
            int id,
            int? ownerId,
            decimal freeBytes,
            decimal totalBytes,
            string driveLetter,
            string fileSystemName)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.FreeBytes = freeBytes;
            this.TotalBytes = totalBytes;
            this.DriveLetter = driveLetter;
            this.FileSystemName = fileSystemName;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? OwnerId { get; set; }

        public decimal FreeBytes { get; set; }

        public decimal TotalBytes { get; set; }

        [NotNullAndEmpty]
        public string DriveLetter { get; set; }

        [NotNull]
        public string FileSystemName { get; set; }

        public decimal Percent
        {
            get
            {
                if (this.TotalBytes == 0)
                {
                    return 0;
                }

                return this.FreeBytes * 100 / this.TotalBytes;
            }
        }
    }
}