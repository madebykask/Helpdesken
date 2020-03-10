namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class Inventory : INewBusinessModel
    {
        protected Inventory(
            int? departmentId,
            int? roomId,
            string name,
            string model,
            string manufacturer,
            string serialNumber,
            string theftMark,
            int? computerTypeId,
            string barCode,
            DateTime? purchaseDate,
            string info)
        {
            this.DepartmentId = departmentId;
            this.RoomId = roomId;
            this.Name = name;
            this.Model = model;
            this.Manufacturer = manufacturer;
            this.SerialNumber = serialNumber;
            this.TheftMark = theftMark;
            this.ComputerTypeId = computerTypeId;
            this.BarCode = barCode;
            this.PurchaseDate = purchaseDate;
            this.Info = info;
        }

        public int Id { get; set; }

        [IsId]
        public int? DepartmentId { get; private set; }

        [IsId]
        public int? RoomId { get; private set; }

        public string Name { get; private set; }

        public string Model { get; private set; }

        public string Manufacturer { get; private set; }

        public string SerialNumber { get; private set; }

        public string TheftMark { get; private set; }

        [IsId]
        public int? ComputerTypeId { get; private set; }

        public string BarCode { get; private set; }

        public DateTime? PurchaseDate { get; private set; }

        public string Info { get; private set; }
    }
}