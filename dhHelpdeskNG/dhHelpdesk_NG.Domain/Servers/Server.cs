namespace DH.Helpdesk.Domain.Servers
{
    using global::System;

    using DH.Helpdesk.Domain.Interfaces;
    using DH.Helpdesk.Domain.WorkstationModules;

    public class Server : Entity, ICustomerEntity
    {
        public int? ChangedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? NIC_Id { get; set; }
        public int? OperatingSystem_Id { get; set; }
        public int? Processor_Id { get; set; }
        public int? RAM_Id { get; set; }
        public int? Room_Id { get; set; }
        public string BarCode { get; set; }
        public string ChassisType { get; set; }
        public string Harddrive { get; set; }
        public string Info { get; set; }
        public string IPAddress { get; set; }
        public string Location { get; set; }
        public string MACAddress { get; set; }
        public string Manufacturer { get; set; }
        public string Miscellaneous { get; set; }
        public string Owner { get; set; }
        public string ServerFileName { get; set; }
        public byte[] ServerDocument { get; set; }
        public string ProductKey { get; set; }
        public string RegistrationCode { get; set; }
        public string SerialNumber { get; set; }
        public string ServerDescription { get; set; }
        public string ServerModel { get; set; }
        public string ServerName { get; set; }
        public string SP { get; set; }
        public string Version { get; set; }
        public string URL { get; set; }
        public string URL2 { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? Scandate { get; set; }
        public DateTime? SyncChangedDate { get; set; }

        public Customer Customer { get; set; }
        public NIC NIC { get; set; }
        public global::DH.Helpdesk.Domain.WorkstationModules.OperatingSystem OperatingSystem { get; set; }
        public Processor Processor { get; set; }
        public RAM RAM { get; set; }
        public Room Room { get; set; }
    }
}
