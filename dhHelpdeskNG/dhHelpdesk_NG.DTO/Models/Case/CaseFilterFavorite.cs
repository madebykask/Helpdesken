using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public sealed class CaseFilterFavorite
    {
        public CaseFilterFavorite()
        {
            Fields = new CaseFilterFavoriteFields();
            CreatedDate = DateTime.Now;
        }

        public CaseFilterFavorite(int id, int customerId, int userId, string name, CaseFilterFavoriteFields fields,
            DateTime? createdDate = null)
        {
            Id = id;
            CustomerId = customerId;
            UserId = userId;
            Name = name;
            Fields = fields;

            CreatedDate = createdDate ?? DateTime.Now;
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public CaseFilterFavoriteFields Fields { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class CaseFilterFavoriteFields
    {
        #region Constructor

        public CaseFilterFavoriteFields()
        {
            InitiatorFilter = "";
            InitiatorSearchScopeFilter = new SelectedItems();
            RegionFilter = new SelectedItems();
            DepartmentFilter = new SelectedItems();
            CaseTypeFilter = new SelectedItems();
            ProductAreaFilter = new SelectedItems();
            WorkingGroupFilter = new SelectedItems();
            ResponsibleFilter = new SelectedItems();
            AdministratorFilter = new SelectedItems();
            PriorityFilter = new SelectedItems();
            StatusFilter = new SelectedItems();
            SubStatusFilter = new SelectedItems();
            RemainingTimeFilter = new SelectedItems();
            ClosingReasonFilter = new SelectedItems();
            RegisteredByFilter = new SelectedItems();
            RegistrationDateFilter = new DateToDate();
            WatchDateFilter = new DateToDate();
            ClosingDateFilter = new DateToDate();
        }

        public CaseFilterFavoriteFields(
            string initiatorFilter,
            SelectedItems initiatorSearchScopeFilter,
            SelectedItems regionFilter,
            SelectedItems departmentFilter,
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
            SelectedItems registeredByFilter,
            DateToDate registrationDateFilter,
            DateToDate watchDateFilter,
            DateToDate closingDateFilter)
        {
            InitiatorFilter = initiatorFilter;
            InitiatorSearchScopeFilter = initiatorSearchScopeFilter;
            RegionFilter = regionFilter;
            DepartmentFilter = departmentFilter;
            CaseTypeFilter = caseTypeFilter;
            ProductAreaFilter = productAreaFilter;
            WorkingGroupFilter = workingGroupFilter;
            ResponsibleFilter = responsibleFilter;
            AdministratorFilter = administratorFilter;
            PriorityFilter = priorityFilter;
            StatusFilter = statusFilter;
            SubStatusFilter = subStatusFilter;
            RemainingTimeFilter = remainingTimeFilter;
            RegisteredByFilter = registeredByFilter;
            ClosingReasonFilter = closingReasonFilter;
            RegistrationDateFilter = registrationDateFilter;
            WatchDateFilter = watchDateFilter;
            ClosingDateFilter = closingDateFilter;
        }

        #endregion

        #region Properties

        public string InitiatorFilter { get; set; }

        public SelectedItems InitiatorSearchScopeFilter { get; set; }

        public SelectedItems RegionFilter { get; set; }

        public SelectedItems DepartmentFilter { get; set; }

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

        public SelectedItems RegisteredByFilter { get; set; }

        public DateToDate RegistrationDateFilter { get; set; }

        public DateToDate WatchDateFilter { get; set; }

        public DateToDate ClosingDateFilter { get; set; }

        #endregion

        public IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                [nameof(InitiatorFilter)] = InitiatorFilter,
                [nameof(InitiatorSearchScopeFilter)] = InitiatorSearchScopeFilter.GetSelectedStrOrNull(),
                [nameof(RegionFilter)] = RegionFilter.GetSelectedStrOrNull(),
                [nameof(DepartmentFilter)] = DepartmentFilter.GetSelectedStrOrNull(),
                [nameof(CaseTypeFilter)] = CaseTypeFilter.GetSelectedStrOrNull(),
                [nameof(ProductAreaFilter)] = ProductAreaFilter.GetSelectedStrOrNull(),
                [nameof(WorkingGroupFilter)] = WorkingGroupFilter.GetSelectedStrOrNull(),
                [nameof(ResponsibleFilter)] = ResponsibleFilter.GetSelectedStrOrNull(),
                [nameof(AdministratorFilter)] = AdministratorFilter.GetSelectedStrOrNull(),
                [nameof(PriorityFilter)] = PriorityFilter.GetSelectedStrOrNull(),
                [nameof(StatusFilter)] = StatusFilter.GetSelectedStrOrNull(),
                [nameof(SubStatusFilter)] = SubStatusFilter.GetSelectedStrOrNull(),
                [nameof(RemainingTimeFilter)] = RemainingTimeFilter.GetSelectedStrOrNull(),
                [nameof(ClosingReasonFilter)] = ClosingReasonFilter.GetSelectedStrOrNull(),
                [nameof(RegisteredByFilter)] = RegisteredByFilter.GetSelectedStrOrNull(),
                [nameof(RegistrationDateFilter)] = RegistrationDateFilter.GetDateString(),
                [nameof(WatchDateFilter)] = WatchDateFilter.GetDateString(),
                [nameof(ClosingDateFilter)] = ClosingDateFilter.GetDateString()
            };
        }

    }
}