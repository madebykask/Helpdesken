namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    using System;

    public class DateFieldsSettings
    {
        public DateFieldsSettings(DateTime cretatedDate, DateTime changedDate, DateTime synchronizeDate, DateTime scanDate, string pathDirectory)
        {
            this.CretatedDate = cretatedDate;
            this.ChangedDate = changedDate;
            this.SynchronizeDate = synchronizeDate;
            this.ScanDate = scanDate;
            this.PathDirectory = pathDirectory;
        }

        public DateTime CretatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime SynchronizeDate { get; set; }

        public DateTime ScanDate { get; set; }

        public string PathDirectory { get; set; }
    }
}