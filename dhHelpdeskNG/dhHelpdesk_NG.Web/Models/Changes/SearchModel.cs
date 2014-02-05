namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DataAnnotationsExtensions;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;
    using dhHelpdesk_NG.Web.Models.Common;

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
            SearchDropDownModel<MultiSelectList> statusesDropDown,
            SearchDropDownModel<MultiSelectList> objectsDropDown,
            SearchDropDownModel<MultiSelectList> ownersDropDown,
            SearchDropDownModel<MultiSelectList> workingGroupsDropDown,
            SearchDropDownModel<MultiSelectList> administratorsDropDown,
            string pharse, 
            SelectList status,
            int recordsOnPage)
        {
            this.StatusesDropDown = statusesDropDown;
            this.ObjectsDropDown = objectsDropDown;
            this.OwnersDropDown = ownersDropDown;
            this.WorkingGroupsDropDown = workingGroupsDropDown;
            this.AdministratorsDropDown = administratorsDropDown;
            this.Pharse = pharse;
            this.ShowItems = status;
            this.RecordsOnPage = recordsOnPage;
        }

        [NotNull]
        public SearchDropDownModel<MultiSelectList> StatusesDropDown { get; set; }
        
        [NotNull]
        public List<int> StatusIds { get; set; }

        [NotNull]
        [LocalizedDisplay("Objects")]
        public SearchDropDownModel<MultiSelectList> ObjectsDropDown { get; set; }

        [NotNull]
        public List<int> ObjectIds { get; set; }

        [NotNull]
        [LocalizedDisplay("Owners")]
        public SearchDropDownModel<MultiSelectList> OwnersDropDown { get; set; }

        [NotNull]
        public List<int> OwnerIds { get; set; }

        [NotNull]
        [LocalizedDisplay("Working groups")]
        public SearchDropDownModel<MultiSelectList> WorkingGroupsDropDown { get; set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        [LocalizedDisplay("Administrators")]
        public SearchDropDownModel<MultiSelectList> AdministratorsDropDown { get; set; }
        
        [NotNull]
        public List<int> AdministratorIds { get; set; }

        [LocalizedDisplay("Search")]
        public string Pharse { get; set; }

        [LocalizedDisplay("Show")]
        public SelectList ShowItems { get; set; }

        public ChangeStatus ShowValue { get; set; }

        [Min(0)]
        [LocalizedDisplay("Records on Page")]
        public int RecordsOnPage { get; set; }
    }
}