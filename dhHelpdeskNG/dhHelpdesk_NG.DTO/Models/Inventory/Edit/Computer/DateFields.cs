namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    public class DateFields
    {
        public DateFields(DateTime synchronizeDate, DateTime scanDate, string pathDirectory)
        {
            this.SynchronizeDate = synchronizeDate;
            this.ScanDate = scanDate;
            this.PathDirectory = pathDirectory;
        }

        public DateTime SynchronizeDate { get; set; }

        public DateTime ScanDate { get; set; }

        public string PathDirectory { get; set; }
    }
}