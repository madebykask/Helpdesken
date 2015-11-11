namespace DH.Helpdesk.BusinessData.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Shared;

    public sealed class CaseFilterFavorite
    {
        public CaseFilterFavorite()
        {
            this.Fields = new CaseFilterFavoriteFields();
        }

        public CaseFilterFavorite(int id, int customerId, int userId, string name, CaseFilterFavoriteFields fields)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.UserId = userId;
            this.Name = name;
            this.Fields = fields;
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }       

        public CaseFilterFavoriteFields Fields { get; set; }
    }

    public class CaseFilterFavoriteFields
    {
        #region Constructor

        public CaseFilterFavoriteFields()
        {
            this.RegionFilter = new SelectedItems();
            this.DepartmentFilter = new SelectedItems();
            this.RegisteredByFilter = new SelectedItems();
            this.CaseTypeFilter = new SelectedItems();
            this.ProductAreaFilter = new SelectedItems();
            this.WorkingGroupFilter = new SelectedItems();
            this.ResponsibleFilter = new SelectedItems();
            this.AdministratorFilter = new SelectedItems();
            this.PriorityFilter = new SelectedItems();
            this.StatusFilter = new SelectedItems();
            this.SubStatusFilter = new SelectedItems();
            this.RemainingTimeFilter = new SelectedItems();
            this.ClosingReasonFilter = new SelectedItems();
            this.RegistrationDateFilter = new DateToDate();
            this.WatchDateFilter = new DateToDate();
            this.ClosingDateFilter = new DateToDate();            
        }

        public CaseFilterFavoriteFields(
                    SelectedItems regionFilter,
                    SelectedItems departmentFilter,
                    SelectedItems registeredByFilter,
                    SelectedItems caseTypeFilter,
                    SelectedItems productAreaFilter,
                    SelectedItems workingGroupFilter,
                    SelectedItems responsibleFilter,
                    SelectedItems administratorFilter,
                    SelectedItems priorityFilter,
                    SelectedItems statusFilter,
                    SelectedItems subStatusFilter,
                    SelectedItems remainingTimeFilter,
                    SelectedItems closingReasonFilter,
                    DateToDate registrationDateFilter,
                    DateToDate watchDateFilter,
                    DateToDate closingDateFilter)
        {
            this.RegionFilter = regionFilter;
            this.DepartmentFilter = new SelectedItems();
            this.RegisteredByFilter = new SelectedItems();
            this.CaseTypeFilter = new SelectedItems();
            this.ProductAreaFilter = new SelectedItems();
            this.WorkingGroupFilter = new SelectedItems();
            this.ResponsibleFilter = new SelectedItems();
            this.AdministratorFilter = new SelectedItems();
            this.PriorityFilter = new SelectedItems();
            this.StatusFilter = new SelectedItems();
            this.SubStatusFilter = new SelectedItems();
            this.RemainingTimeFilter = new SelectedItems();
            this.ClosingReasonFilter = new SelectedItems();
            this.RegistrationDateFilter = new DateToDate();
            this.WatchDateFilter = new DateToDate();
            this.ClosingDateFilter = new DateToDate();
        }

        #endregion

        #region Properties

        public SelectedItems RegionFilter { get; set; }

        public SelectedItems DepartmentFilter { get; set; }

        public SelectedItems RegisteredByFilter { get; set; }

        public SelectedItems CaseTypeFilter { get; set; }

        public SelectedItems ProductAreaFilter { get; set; }

        public SelectedItems WorkingGroupFilter { get; set; }

        public SelectedItems ResponsibleFilter { get; set; }

        public SelectedItems AdministratorFilter { get; set; }

        public SelectedItems PriorityFilter { get; set; }

        public SelectedItems StatusFilter { get; set; }

        public SelectedItems SubStatusFilter { get; set; }

        public SelectedItems RemainingTimeFilter { get; set; }

        public SelectedItems ClosingReasonFilter { get; set; }

        public DateToDate RegistrationDateFilter { get; set; }

        public DateToDate WatchDateFilter { get; set; }

        public DateToDate ClosingDateFilter { get; set; }
                

        #endregion
    }

}