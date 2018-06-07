using System;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.BusinessData.Models.Contract
{
    public class ContractHistoryFull
    {
        public int ContractId { get; set; }
        public string ContractNumber { get; set; }
        public int Finished { get; set; }
        public int FollowUpInterval { get; set; }
        public int Running { get; set; }
        public string Info { get; set; }
        public string Files { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NoticeDate { get; set; }
        public int NoticeTime { get; set; }
        public UserBasicOvierview CreatedByUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public EntityOverview Supplier { get; set; }
        public EntityOverview ContractCategory { get; set; }
        public EntityOverview Department { get; set; }
        public UserBasicOvierview FollowUpResponsibleUser { get; set; }
        public UserBasicOvierview ResponsibleUser { get; set; }
    }
}