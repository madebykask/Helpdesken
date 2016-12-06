namespace DH.Helpdesk.BusinessData.Models.Contract
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using System;
    using System.Collections.Generic;


    public class ContractFileModel : INewBusinessModel
    {
        public ContractFileModel() { }

        public ContractFileModel(
                int id,
                int contractId,
                byte[] content,
                int? archivedContractFileId,
                string contentType,              
                string fileName,
                DateTime? archivedDate,
                DateTime createdDate,
                Guid contractFileGuid
            )
        {
            this.Id = id;
            this.Contract_Id =  contractId;
            this.Content = content;
            this.ArchivedContractFile_Id = archivedContractFileId;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.ArchivedDate = archivedDate;
            this.CreatedDate = createdDate;
            this.ContractFileGuid = contractFileGuid;
        }

        public int Id { get; set; }

        public byte[] Content { get; set; }

        public int Contract_Id { get; set; }            

        public int? ArchivedContractFile_Id { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public DateTime? ArchivedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid ContractFileGuid { get; set; }

    }
}