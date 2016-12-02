namespace DH.Helpdesk.Web.Models.Contract
{
    using System;

    public class ContractFileViewModel 
    {
        public ContractFileViewModel() { }

        public ContractFileViewModel(
                int id,
                int contractId,
                byte[] content,
                int? archivedContractFileId,
                string contractType,              
                string fileName,
                DateTime? archivedDate,
                DateTime createdDate,
                string contractFileKey,
                Guid contractFileGuid
            )
        {
            this.Id = id;
            this.Contract_Id =  contractId;
            this.Content = content;
            this.ArchivedContractFile_Id = archivedContractFileId;
            this.ContractType = contractType;
            this.FileName = fileName;
            this.ArchivedDate = archivedDate;
            this.CreatedDate = createdDate;
            this.ContractFileKey = contractFileKey;
            this.ContractFileGuid = contractFileGuid;
        }

        public int Id { get; set; }

        public byte[] Content { get; set; }

        public int Contract_Id { get; set; }            

        public int? ArchivedContractFile_Id { get; set; }

        public string ContractType { get; set; }

        public string FileName { get; set; }

        public DateTime? ArchivedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid ContractFileGuid { get; set; }

        public string ContractFileKey { get; set; }

    }
}