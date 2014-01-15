namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Data.Enums.Changes;

    public sealed class SearchModel
    {
        public SearchModel()
        {
            this.StatusIds = new List<int>();
            this.ObjectIds = new List<int>();
            this.OwnerIds = new List<int>();
            this.ProcessAffectedIds = new List<int>();
            this.WorkingGroupIds = new List<int>();
            this.AdministratorIds = new List<int>();
        }

        public SearchModel(string pharse, ChangeStatus status, int recordsOnPage)
        {
            this.Pharse = pharse;
            this.Status = status;
            this.RecordsOnPage = recordsOnPage;
        }

        [NotNull]
        public List<int> StatusIds { get; set; }

        [NotNull]
        public List<int> ObjectIds { get; set; }

        [NotNull]
        public List<int> OwnerIds { get; set; }

        [NotNull]
        public List<int> ProcessAffectedIds { get; set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public List<int> AdministratorIds { get; set; }

        [Min(0)]
        public int RecordsOnPage { get; set; }

        public string Pharse { get; set; }

        public ChangeStatus Status { get; set; }
    }
}