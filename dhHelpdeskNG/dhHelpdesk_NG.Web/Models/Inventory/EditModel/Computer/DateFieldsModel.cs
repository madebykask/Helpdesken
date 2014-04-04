namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;

    public class DateFieldsModel
    {
        public DateFieldsModel(
            ConfigurableFieldModel<DateTime> synchronizeDate,
            ConfigurableFieldModel<DateTime> scanDate,
            ConfigurableFieldModel<string> pathDirectory)
        {
            this.SynchronizeDate = synchronizeDate;
            this.ScanDate = scanDate;
            this.PathDirectory = pathDirectory;
        }

        public ConfigurableFieldModel<DateTime> SynchronizeDate { get; set; }

        public ConfigurableFieldModel<DateTime> ScanDate { get; set; }

        public ConfigurableFieldModel<string> PathDirectory { get; set; }
    }
}