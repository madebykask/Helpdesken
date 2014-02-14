namespace DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeDetailedOverview
{
    using System;

    public sealed class GeneralFields
    {
        public GeneralFields(
            int? priority,
            string title,
            string state,
            string system,
            string @object,
            string inventory,
            string workingGroup,
            UserName administrator,
            DateTime? finishingDate,
            bool rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.State = state;
            this.System = system;
            this.Object = @object;
            this.Inventory = inventory;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
            this.FinishingDate = finishingDate;
            this.Rss = rss;
        }

        public int? Priority { get; private set; }

        public string Title { get; private set; }

        public string State { get; private set; }

        public string System { get; private set; }

        public string Object { get; private set; }

        public string Inventory { get; private set; }

        public string WorkingGroup { get; private set; }

        public UserName Administrator { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool Rss { get; private set; }
    }
}
