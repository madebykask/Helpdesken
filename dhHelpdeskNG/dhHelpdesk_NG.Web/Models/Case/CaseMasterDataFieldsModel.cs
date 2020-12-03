namespace DH.Helpdesk.Web.Models.Case
{    
    public sealed class CaseMasterDataFieldsModel
    {
        public CaseMasterDataFieldsModel()
        {           
        }

        public int CustomerId { get; set; }
        public int? RegionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? OUId { get; set; }
        public int? SourceId { get; set; }
        public int? CaseTypeId { get; set; }
        public int? ProductAreaId { get; set; }
        public int? SystemId { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public int? WorkingGroupId { get; set; }
        public int? ResponsibleId { get; set; }
        public int? AdministratorId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public int? CausingPartId { get; set; }
        public int? ClosingReasonId { get; set; }
                
    }
}