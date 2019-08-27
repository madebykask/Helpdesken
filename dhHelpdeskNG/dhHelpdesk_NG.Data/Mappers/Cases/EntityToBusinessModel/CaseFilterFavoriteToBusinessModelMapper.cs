using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    public sealed class CaseFilterFavoriteToBusinessModelMapper : IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite>
    {
        public CaseFilterFavorite Map(CaseFilterFavoriteEntity entity)
        {           
            var fields = new CaseFilterFavoriteFields(
                    new SelectedItems(entity.RegionFilter, false),
                    new SelectedItems(entity.DepartmentFilter, false),
                    new SelectedItems(entity.CaseTypeFilter, false),
                    new SelectedItems(entity.ProductAreaFilter, false),
                    new SelectedItems(entity.WorkingGroupFilter, false),
                    new SelectedItems(entity.ResponsibleFilter, false),
                    new SelectedItems(entity.AdministratorFilter, false),
                    new SelectedItems(entity.PriorityFilter, false),
                    new SelectedItems(entity.StatusFilter, false),
                    new SelectedItems(entity.SubStatusFilter, false),
                    new SelectedItems(entity.RemainingTimeFilter, false),
                    new SelectedItems(entity.ClosingReasonFilter, false),
                    new SelectedItems(entity.RegisteredByFilter, false),
                    new DateToDate(entity.RegistrationDateStartFilter, entity.RegistrationDateEndFilter),
                    new DateToDate(entity.WatchDateStartFilter, entity.WatchDateEndFilter),
                    new DateToDate(entity.ClosingDateStartFilter, entity.ClosingDateEndFilter));

            var caseFilterFavorite = new CaseFilterFavorite(            
                                                        entity.Id, 
                                                        entity.Customer_Id,
                                                        entity.User_Id,
                                                        entity.Name,
                                                        fields,
                                                        entity.CreatedDate);
            return caseFilterFavorite;
        }
    }
}