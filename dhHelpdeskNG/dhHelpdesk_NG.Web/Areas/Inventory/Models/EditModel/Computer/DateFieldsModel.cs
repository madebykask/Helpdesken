namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class DateFieldsModel
    {
        public DateFieldsModel()
        {
        }

        public DateFieldsModel(
            ConfigurableFieldModel<DateTime?> synchronizeDate,
            ConfigurableFieldModel<DateTime?> scanDate,
            ConfigurableFieldModel<string> pathDirectory)
        {
            this.SynchronizeDate = synchronizeDate;
            this.ScanDate = scanDate;
            this.PathDirectory = pathDirectory;
        }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> SynchronizeDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> ScanDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> PathDirectory { get; set; }
    }
}