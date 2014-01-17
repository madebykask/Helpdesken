namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public sealed class SearchModel
    {
        public SearchModel()
        {
            this.StatusIds = new List<int>();
            this.ObjectIds = new List<int>();
            this.OwnerIds = new List<int>();
            this.WorkingGroupIds = new List<int>();
            this.AdministratorIds = new List<int>();
        }

        public SearchModel(
            MultiSelectList statusItems, 
            MultiSelectList objectItems,
            MultiSelectList workingGroupItems,
            MultiSelectList administratorItems,
            string pharse, 
            SelectList status,
            int recordsOnPage)
        {
            this.StatusItems = statusItems;
            this.ObjectItems = objectItems;
            this.WorkingGroupItems = workingGroupItems;
            this.AdministratorItems = administratorItems;
            this.Pharse = pharse;
            this.ShowItems = status;
            this.RecordsOnPage = recordsOnPage;
        }

        [NotNull]
        [LocalizedDisplay("Statuses")]
        public MultiSelectList StatusItems { get; set; }

        [NotNull]
        public List<int> StatusIds { get; set; }

        [NotNull]
        public List<int> ObjectIds { get; set; }

        [NotNull]
        public List<int> OwnerIds { get; set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public List<int> AdministratorIds { get; set; }

        [NotNull]
        [LocalizedDisplay("Objects")]
        public MultiSelectList ObjectItems { get; set; }

        [NotNull]
        [LocalizedDisplay("Owners")]
        public MultiSelectList OwnerItems { get; set; }

        [NotNull]
        [LocalizedDisplay("Working groups")]
        public MultiSelectList WorkingGroupItems { get; set; }

        [NotNull]
        [LocalizedDisplay("Administrators")]
        public MultiSelectList AdministratorItems { get; set; }

        [Min(0)]
        [LocalizedDisplay("Records on Page")]
        public int RecordsOnPage { get; set; }

        [LocalizedDisplay("Search")]
        public string Pharse { get; set; }

        [LocalizedDisplay("Show")]
        public SelectList ShowItems { get; set; }

        public ChangeStatus ShowValue { get; set; }
    }
}