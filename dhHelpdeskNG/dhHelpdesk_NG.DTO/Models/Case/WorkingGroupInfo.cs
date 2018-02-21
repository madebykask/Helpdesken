using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class WorkingGroupInfo
    {
        public int Id { get; set; }
        public int WorkingGroupId { get; set; }
        public Guid? WorkingGroupGuid { get; set; }
    }
}