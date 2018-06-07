namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    using System;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryOverview
    {
        public InventoryOverview(
            int id,
            string departmentName,
            string roomName,
            UserName changeByUser,
            string name,
            string model,
            string manufacturer,
            string serialNumber,
            string theftMark,
            string barCode,
            DateTime? purchaseDate,
            string workstationName,
            string info,
            DateTime createdDate,
            DateTime changedDate,
            DateTime? syncChangeDate)
        {
            this.Id = id;
            this.DepartmentName = departmentName;
            this.RoomName = roomName;
            this.ChangeByUser = changeByUser;
            this.Name = name;
            this.Model = model;
            this.Manufacturer = manufacturer;
            this.SerialNumber = serialNumber;
            this.TheftMark = theftMark;
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
            this.WorkstationName = workstationName;
            this.Info = info;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SyncChangedDate = syncChangeDate;
        }

        [IsId]
        public int Id { get; private set; }

        public string DepartmentName { get; private set; }

        public string RoomName { get; private set; }

        public UserName ChangeByUser { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        public string Model { get; private set; }

        [NotNull]
        public string Manufacturer { get; private set; }

        [NotNull]
        public string SerialNumber { get; private set; }

        [NotNull]
        public string TheftMark { get; private set; }

        [NotNull]
        public string BarCode { get; private set; }

        public DateTime? PurchaseDate { get; private set; }

        public string WorkstationName { get; private set; }

        public string Info { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime? SyncChangedDate { get; private set; }
    }
}
