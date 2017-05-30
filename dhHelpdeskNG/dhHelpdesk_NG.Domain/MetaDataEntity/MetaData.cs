using System;

namespace DH.Helpdesk.Domain.MetaDataEntity
{
    public class MetaData: Entity
    {
        public int Customer_Id { get; set; }

        public Guid MetaDataGuid { get; set; }

        public Guid EntityInfo_Guid { get; set; }

        public string MetaDataCode { get; set; }

        public string MetaDataText { get; set; }

        public string MetaDataDescription { get; set; }

        public int? ExtenalId { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate  { get; set; }

        public DateTime SynchronizedDate  { get; set; }

    }
}
