using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.Enums.Condition;
using System;

namespace DH.Helpdesk.BusinessData.Models.Condition
{
    public class ConditionModel : INewBusinessModel
    {
        public ConditionModel()
        {
        }

        public int Id { get; set; }
        public int ConditionType_Id { get; set; }
        public int Parent_Id { get; set; } //Connected to the specific ConditionType
        public Guid GUID { get; set; }

        public string Name { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
        public ConditionOperator Operator { get; set; }  
        public string Description { get; set; }

        //TODO: place below in base class model so we dont need to add this on all places
        public int Status { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public int? ChangedByUser_Id { get; set; }
    }
}