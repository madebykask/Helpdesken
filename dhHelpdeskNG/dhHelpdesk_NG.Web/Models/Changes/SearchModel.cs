namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Common;

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
            int recordsOnPage) : this()
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
        }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Statuses { get; private set; }
        
        [NotNull]
        public List<int> StatusIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Objects { get; private set; }

        [NotNull]
        public List<int> ObjectIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Owners { get; private set; }

        [NotNull]
        public List<int> OwnerIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> AffectedProcesses { get; private set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> WorkingGroups { get; private set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public ConfigurableSearchFieldModel<MultiSelectList> Administrators { get; private set; }
        
        [NotNull]
        public List<int> AdministratorIds { get; set; }

        [LocalizedDisplay("Pharse")]
        public string Pharse { get; set; }

        [LocalizedDisplay("Show")]
        public SelectList Status { get; private set; }

        public ChangeStatus? StatusValue { get; set; }

        [Min(0)]
        // ToDo: Mark as integer.
        [LocalizedDisplay("Records on Page")]
        public int RecordsOnPage { get; set; }

        public ChangesFilter ExtractFilters()
        {
            return new ChangesFilter(
                this.StatusIds,
                this.ObjectIds,
                this.OwnerIds,
                this.AffectedProcessIds,
                this.WorkingGroupIds,
                this.AdministratorIds,
                this.Pharse,
                this.StatusValue,
                this.RecordsOnPage);
        }
    }
}