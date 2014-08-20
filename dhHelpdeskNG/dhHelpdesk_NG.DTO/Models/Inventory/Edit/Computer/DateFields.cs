namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.Common.Types;

    public class DateFields
    {
        public DateFields(
            DateTime? synchronizeDate,
            DateTime? scanDate,
            string pathDirectory,
            UserName createdByUserName)
        {
            this.SynchronizeDate = synchronizeDate;
            this.ScanDate = scanDate;
            this.PathDirectory = pathDirectory;
            this.ChangedByUserName = createdByUserName;
        }

        public DateTime? SynchronizeDate { get; set; }

        public DateTime? ScanDate { get; set; }

        public string PathDirectory { get; set; }

        public UserName ChangedByUserName { get; set; }
    }
}