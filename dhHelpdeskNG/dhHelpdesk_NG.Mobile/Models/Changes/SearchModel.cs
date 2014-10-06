namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class SearchModel : ISearchModel<ChangesFilter>
    {
        public SearchModel()
        {
            this.StatusIds = new List<int>();
            this.ObjectIds = new List<int>();
            this.OwnerIds = new List<int>();
            this.AffectedProcessIds = new List<int>();
            this.WorkingGroupIds = new List<int>();
            this.AdministratorIds = new List<int>();
        }

        public SearchModel(
            ConfigurableSearchFieldModel<MultiSelectList> statuses,
            ConfigurableSearchFieldModel<MultiSelectList> objects,
            ConfigurableSearchFieldModel<MultiSelectList> owners,
            ConfigurableSearchFieldModel<MultiSelectList> affectedProcesses,
            ConfigurableSearchFieldModel<MultiSelectList> workingGroups,
            ConfigurableSearchFieldModel<MultiSelectList> administrators,
            string pharse,
            SelectList status,
            int recordsOnPage,
            SortFieldModel sortField)
        {
            this.Statuses = statuses;
            this.Objects = objects;
            this.Owners = owners;
            this.AffectedProcesses = affectedProcesses;
            this.WorkingGroups = workingGroups;
            this.Administrators = administrators;
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
            this.SortField = sortField;
        }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Statuses { get; set; }

        [NotNull]
        public List<int> StatusIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Objects { get; set; }

        [NotNull]
        public List<int> ObjectIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Owners { get; set; }

        [NotNull]
        public List<int> OwnerIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> AffectedProcesses { get; set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> WorkingGroups { get; set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Administrators { get; set; }

        [NotNull]
        public List<int> AdministratorIds { get; set; }

        [LocalizedDisplay("Phrase")]
        public string Pharse { get; set; }

        [LocalizedDisplay("Show")]
        public SelectList Status { get; set; }

        public ChangeStatus? StatusValue { get; set; }

        [LocalizedDisplay("Records on Page")]
        [LocalizedInteger]
        [LocalizedMin(0)]
        public int RecordsOnPage { get; set; }

        public SortFieldModel SortField { get; set; }

        public ChangesFilter ExtractFilters()
        {
            SortField sortField = null;

            if (!string.IsNullOrEmpty(SortField.Name))
            {
                sortField = new SortField(SortField.Name, SortField.SortBy.Value);
            }

            return new ChangesFilter(
                this.StatusIds,
                this.ObjectIds,
                this.OwnerIds,
                this.AffectedProcessIds,
                this.WorkingGroupIds,
                this.AdministratorIds,
                this.Pharse,
                this.StatusValue,
                this.RecordsOnPage,
                sortField);
        }
    }
}