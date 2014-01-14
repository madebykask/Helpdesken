namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    using System;

    public sealed class GeneralFieldGroupDto
    {
        public GeneralFieldGroupDto(
            int? priority,
            string title,
            string state,
            string system,
            string @object,
            string inventory,
            string owner,
            string workingGroup,
            string administrator,
            DateTime? finishingDate,
            bool rss)
        {
            Priority = priority;
            Title = title;
            State = state;
            System = system;
            Object = @object;
            Inventory = inventory;
            Owner = owner;
            WorkingGroup = workingGroup;
            Administrator = administrator;
            FinishingDate = finishingDate;
            Rss = rss;
        }

        public int? Priority { get; private set; }

        public string Title { get; private set; }

        public string State { get; private set; }

        public string System { get; private set; }

        public string Object { get; private set; }

        public string Inventory { get; private set; }

        public string Owner { get; private set; }

        public string WorkingGroup { get; private set; }

        public string Administrator { get; private set; }

        public DateTime? FinishingDate { get; private set; }

        public bool Rss { get; private set; }
    }
}
