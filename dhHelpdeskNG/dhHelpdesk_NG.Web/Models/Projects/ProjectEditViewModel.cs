﻿namespace DH.Helpdesk.Web.Models.Projects
{
    using System.Web.Mvc;

    public class ProjectEditViewModel
    {
        public ProjectEditModel ProjectEditModel { get; set; }

        public MultiSelectList Users { get; set; }

        public string Guid { get; set; }
    }
}