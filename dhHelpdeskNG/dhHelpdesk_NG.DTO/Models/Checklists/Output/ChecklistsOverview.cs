namespace DH.Helpdesk.BusinessData.Models.Checklists.Output
{
    using System;

    using DH.Helpdesk.Domain;

    public sealed class ChecklistsOverview
    {
        public ChecklistsOverview(
                DateTime createdDate,
                string checklistName)
        {
           
            this.CreatedDate = createdDate;
            this.ChecklistName = checklistName;
        }

        public DateTime CreatedDate { get; private set; }

        public string ChecklistName { get; private set; }
    }
}
