namespace DH.Helpdesk.Domain.Printers
{
    using global::System;
    using global::System.Collections.Generic;

    public class Printer : Entity
    {
        public int? ChangedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? Room_Id { get; set; }
        public string BarCode { get; set; }
        public string DriverName { get; set; }
        public string Info { get; set; }
        public string IPAddress { get; set; }
        public string Location { get; set; }
        public string MACAddress { get; set; }
        public string Manufacturer { get; set; }
        public string NumberOfTrays { get; set; }
        public string OU { get; set; }
        public string PrinterName { get; set; }
        public string PrinterServer { get; set; }
        public string PrinterType { get; set; }
        public string SerialNumber { get; set; }
        public string Theftmark { get; set; }
        public string URL { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SyncChangedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Room Room { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<OperationObject> OperationObjects { get; set; }
    }
}
