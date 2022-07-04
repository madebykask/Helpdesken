using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{
    public class ComputerDB
    {
        
        public int Id { get; set; }

        public string ComputerGUID { get; set; }

        public int Customer_Id { get; set; }

        public string ComputerName { get; set; }

        public string Manufacturer { get; set; }

        public string ComputerModel { get; set; }

        public int ComputerModel_Id { get; set; }

        public string SerialNumber { get; set; }

        public string BIOSVersion { get; set; }

        public DateTime? BIOSDate { get; set; }

        public string Theftmark { get; set; }

        public string CarePackNumber { get; set; }

        public int ComputerType_Id { get; set; }

        public string ChassisType { get; set; }

        public string Location { get; set; }

        public string BarCode { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public int OS_Id { get; set; }

        public string OS_Version { get; set; }

        public string OS_SP { get; set; }

        public int Processor_Id { get; set; }

        public string ProcessorInfo { get; set; }

        public int RAM_Id { get; set; }

        public int NIC_Id { get; set; }

        public string IPAddress { get; set; }

        public string MACAddress { get; set; }

        public int RAS { get; set; }

        public string NovellClient { get; set; }

        public string Harddrive { get; set; }

        public string VideoCard { get; set; }

        public string SoundCard { get; set; }

        public string MonitorModel { get; set; }

        public string MonitorSerialNumber { get; set; }

        public string MonitorTheftMark { get; set; }

        public string ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int Price { get; set; }

        public string AccountingDimension1 { get; set; }

        public string AccountingDimension2 { get; set; }

        public string AccountingDimension3 { get; set; }

        public string AccountingDimension4 { get; set; }

        public string AccountingDimension5 { get; set; }

        public string ComputerFileName { get; set; }

        public string Info { get; set; }

        public int User_Id { get; set; }

        public string LoggedUser { get; set; }

        public int Room_Id { get; set; }

        public int Status { get; set; }

        public int Stolen { get; set; }

        public string ReplacedWithComputerName { get; set; }

        public int SendBack { get; set; }

        public DateTime? ScrapDate { get; set; }

        public int Department_Id { get; set; }

        public int OU_Id { get; set; }

        public int Domain_Id { get; set; }

        public int ComputerRole { get; set; }

        public string LDAPPath { get; set; }

        public string RegistrationCode { get; set; }

        public string ProductKey { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string LocationAddress { get; set; }

        public string LocationPostalCode { get; set; }

        public string LocationPostalAddress { get; set; }

        public string LocationRoom { get; set; }

        public string Location2 { get; set; }

        public DateTime? ScanDate { get; set; }

        public int Updated { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int RegUser_Id { get; set; }

        public int ChangedByUser_Id { get; set; }

        public DateTime? ChangedDate { get; set; }

        public DateTime? SyncChangedDate { get; set; }

        public int ComputerContractStatus_Id { get; set; }

        public int Region_Id { get; set; }

        public DateTime? WarrantyEndDate { get; set; }
    }
}
