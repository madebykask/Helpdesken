namespace DH.Helpdesk.Domain
{
    using global::System;

    public class ContractFile : Entity
    {
        public byte[] File { get; set; }
        public int? ArchivedContractFile_Id { get; set; }
        public int Contract_Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public DateTime? ArchivedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ContractFileGUID { get; set; }

        public virtual Contract Contract { get; set; }
    }
}
