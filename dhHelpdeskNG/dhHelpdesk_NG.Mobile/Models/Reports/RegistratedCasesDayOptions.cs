namespace DH.Helpdesk.Mobile.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public sealed class RegistratedCasesDayOptions
    {
        public RegistratedCasesDayOptions()
        {
            this.CaseTypeIds = new List<int>();
        }

        public RegistratedCasesDayOptions(
                    SelectList departments, 
                    MultiSelectList caseTypes,
                    SelectList workingGroups,
                    SelectList administrators)
        {
            this.Administrators = administrators;
            this.WorkingGroups = workingGroups;
            this.CaseTypes = caseTypes;
            this.Departments = departments;
        }

        [NotNull]
        public SelectList Departments { get; private set; }    

        public int? DepartmentId { get; set; }

        [NotNull]
        public MultiSelectList CaseTypes { get; private set; }

        [NotNull]
        public List<int> CaseTypeIds { get; set; } 

        [NotNull]
        public SelectList WorkingGroups { get; private set; } 

        public int? WorkingGroupId { get; set; }

        [NotNull]
        public SelectList Administrators { get; private set; } 

        public int? AdministratorId { get; set; }

        [LocalizedRequired]
        public DateTime Period { get; set; }

        public bool IsPrint { get; set; }
    }
}