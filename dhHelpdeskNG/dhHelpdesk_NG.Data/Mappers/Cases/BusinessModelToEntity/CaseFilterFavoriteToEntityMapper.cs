namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Case;    
    using DH.Helpdesk.Domain.Cases;    

    public sealed class CaseFilterFavoritToEntityMapper : IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity>
    {
        public void Map(CaseFilterFavorite businessModel, CaseFilterFavoriteEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            entity.Customer_Id = businessModel.CustomerId;
            entity.User_Id = businessModel.UserId;
            entity.Name = businessModel.Name;
            entity.CreatedDate = businessModel.CreatedDate;

            if (businessModel.Fields == null)
            {
                return;
            }

            entity.InitiatorFilter = businessModel.Fields.InitiatorFilter;
            entity.InitiatorSearchScopeFilter = businessModel.Fields.InitiatorSearchScopeFilter.GetSelectedStrOrNull();
            entity.AdministratorFilter = businessModel.Fields.AdministratorFilter.GetSelectedStrOrNull();
            entity.CaseTypeFilter = businessModel.Fields.CaseTypeFilter.GetSelectedStrOrNull();
            entity.ClosingReasonFilter = businessModel.Fields.ClosingReasonFilter.GetSelectedStrOrNull();
            entity.DepartmentFilter = businessModel.Fields.DepartmentFilter.GetSelectedStrOrNull();
            entity.PriorityFilter = businessModel.Fields.PriorityFilter.GetSelectedStrOrNull();
            entity.ProductAreaFilter = businessModel.Fields.ProductAreaFilter.GetSelectedStrOrNull();
            entity.RegionFilter = businessModel.Fields.RegionFilter.GetSelectedStrOrNull();
            entity.RemainingTimeFilter = businessModel.Fields.RemainingTimeFilter.GetSelectedStrOrNull();
            entity.ResponsibleFilter = businessModel.Fields.ResponsibleFilter.GetSelectedStrOrNull();
            entity.StatusFilter = businessModel.Fields.StatusFilter.GetSelectedStrOrNull();
            entity.SubStatusFilter = businessModel.Fields.SubStatusFilter.GetSelectedStrOrNull();
            entity.WorkingGroupFilter = businessModel.Fields.WorkingGroupFilter.GetSelectedStrOrNull();
            entity.RegisteredByFilter = businessModel.Fields.RegisteredByFilter.GetSelectedStrOrNull();
            
            entity.RegistrationDateStartFilter = businessModel.Fields.RegistrationDateFilter.FromDate;
            entity.RegistrationDateEndFilter = businessModel.Fields.RegistrationDateFilter.ToDate;

            entity.WatchDateStartFilter = businessModel.Fields.WatchDateFilter.FromDate;
            entity.WatchDateEndFilter = businessModel.Fields.WatchDateFilter.ToDate;

            entity.ClosingDateStartFilter = businessModel.Fields.ClosingDateFilter.FromDate;
            entity.ClosingDateEndFilter = businessModel.Fields.ClosingDateFilter.ToDate;
            
        }
    }
}