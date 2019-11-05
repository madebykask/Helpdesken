
namespace DH.Helpdesk.Domain.Computers
{
    using DH.Helpdesk.Domain.Interfaces;
    using DH.Helpdesk.Domain.WorkstationModules;

    using global::System;

    public class Computer : Entity, INulableCustomerEntity
    {
        public Computer()
        {
        }

        public int? ChangedByUser_Id { get; set; }
        public int? ComputerModel_Id { get; set; }
        public int ComputerRole { get; set; }
        public int? ComputerType_Id { get; set; }
        public int? Customer_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? Domain_Id { get; set; }
        public int? NIC_ID { get; set; }
        public int? OS_Id { get; set; }
        public int? Processor_Id { get; set; }
        public int? RAM_ID { get; set; }
        public int RAS { get; set; }
        public int? RegUser_Id { get; set; }
        public int? Room_Id { get; set; }
        public int SendBack { get; set; }
        public int Stolen { get; set; }
        public int Updated { get; set; }
        public int? User_Id { get; set; }
        public string BarCode { get; set; }
        public string BIOSVersion { get; set; }
        public string CarePackNumber { get; set; }
        public string ChassisType { get; set; }
        public string ComputerFileName { get; set; }
        public byte[] ComputerDocument { get; set; }
        public string ComputerGUID { get; set; }
        public string ComputerModelName { get; set; }
        public string ComputerName { get; set; }
        public string HardDrive { get; set; }
        public string Info { get; set; }
        public string IPAddress { get; set; }
        public string LDAPPath { get; set; }
        public string Location { get; set; }
        public string LoggedUser { get; set; }
        public string MACAddress { get; set; }
        public string Manufacturer { get; set; }
        public string MonitorModel { get; set; }
        public string MonitorSerialnumber { get; set; }
        public string MonitorTheftMark { get; set; }
        public string NovellClient { get; set; }
        public string ProductKey { get; set; }
        public string ProcessorInfo { get; set; }
        public string RegistrationCode { get; set; }
        public string ReplacedWithComputerName { get; set; }
        public string SerialNumber { get; set; }
        public string SoundCard { get; set; }
        public string SP { get; set; }
        public string TheftMark { get; set; }
        public string Version { get; set; }
        public string VideoCard { get; set; }
        public DateTime? BIOSDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ScanDate { get; set; }
        public DateTime? ScrapDate { get; set; }

        public string ContractNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string LocationAddress { get; set; }
        public string ContactEmailAddress { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationPostalAddress { get; set; }
        public string LocationRoom { get; set; }
        public string Location2 { get; set; }

        public int? ContractStatus_Id { get; set; }

        public string AccountingDimension1 { get; set; }

        public string AccountingDimension2 { get; set; }

        public string AccountingDimension3 { get; set; }

        public string AccountingDimension4 { get; set; }

        public string AccountingDimension5 { get; set; }

        public int Price { get; set; }

        // Status is not nullable and no foreign key to ComputerStatus because it can have 0 instaed of null -- legacy(supportability) of dh4
        public int Status { get; set; } 

        public DateTime? SyncChangedDate { get; set; }

        public int? OU_Id { get; set; }
        public int? Region_Id { get; set; }

        public virtual OU OU { get; set; }
        public virtual Region Region { get; set; }

        public virtual ComputerModel ComputerModel { get; set; }

        public virtual ComputerType ComputerType { get; set; }

        public virtual Department Department { get; set; }

        public virtual Domain Domain { get; set; }

        public virtual NIC NIC { get; set; }

        public virtual global::DH.Helpdesk.Domain.WorkstationModules.OperatingSystem OS { get; set; }

        public virtual Processor Processor { get; set; }

        public virtual RAM RAM { get; set; }

        public virtual Room Room { get; set; }

        public virtual ComputerUser User { get; set; }
        public virtual ComputerStatus ContractStatus { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
