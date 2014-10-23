namespace DH.Helpdesk.Web.Models.Projects
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ProjectEditModel
    {
        public ProjectEditModel()
        {
            this.ProjectCollaboratorIds = new List<int>();
            this.IsActive = true;
        }

        [LocalizedDisplay("Projektnummer")]
        public int Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Projektnamn")]
        public string Name { get; set; }

        [LocalizedDisplay("Projektledare")]
        public int? ProjectManagerId { get; set; }

        [LocalizedDisplay("Status")]
        public bool IsActive { get; set; }

        [LocalizedStringLength(2000)]
        [LocalizedDisplay("Beskrivning")]
        public string Description { get; set; }

        [LocalizedDisplay("Projektdatum")]
        public DateTime? StartDate { get; set; }

        [LocalizedDisplay("Avslutsdatum")]
        public DateTime? EndDate { get; set; }

        [LocalizedDisplay("Projektmedlemmar")]
        public List<int> ProjectCollaboratorIds { get; set; }

        [LocalizedDisplay("Bifogad fil")]
        public List<int> ProjectFileNames { get; set; }
    }
}
