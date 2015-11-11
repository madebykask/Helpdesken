namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain.Cases;

    public sealed class CaseFilterFavoriteToBusinessModelMapper : IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>
    {
        public CaseFilterFavorite Map(CaseFilterFavoriteEntity entity)
        {           
            var fields = new CaseFilterFavoriteFields(
                    new SelectedItems(entity.RegionFilter),
                    new SelectedItems(entity.DepartmentFilter),
                    new SelectedItems(entity.RegisteredByFilter),
                    new SelectedItems(entity.CaseTypeFilter),
                    new SelectedItems(entity.ProductAreaFilter),
                    new SelectedItems(entity.WorkingGroupFilter),
                    new SelectedItems(entity.ResponsibleFilter),
                    new SelectedItems(entity.AdministratorFilter),
                    new SelectedItems(entity.PriorityFilter),
                    new SelectedItems(entity.StatusFilter),
                    new SelectedItems(entity.SubStatusFilter),
                    new SelectedItems(entity.RemainingTimeFilter),
                    new SelectedItems(entity.ClosingReasonFilter),
                    new DateToDate(entity.RegistrationDateStartFilter, entity.RegistrationDateEndFilter),
                    new DateToDate(entity.WatchDateStartFilter, entity.WatchDateEndFilter),
                    new DateToDate(entity.ClosingDateStartFilter, entity.ClosingDateEndFilter) 
                );

            var caseFilterFavorite = new CaseFilterFavorite(            
                                                        entity.Id, 
                                                        entity.Customer_Id,
                                                        entity.User_Id,
                                                        entity.Name,
                                                        fields);
            return caseFilterFavorite;
        }
    }
}