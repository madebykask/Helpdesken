﻿using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    public sealed class CaseTypeOverview
    {
        public CaseTypeOverview()
        {
            SubCaseTypes = new List<CaseTypeOverview>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public string Name { get; set; }

        public int ShowOnExternalPage { get; set; }
        
        //For next release #57742
        public int ShowOnExtPageCases { get; set; }

        public int IsActive { get; set; }

        public bool IsDefault { get; set; }
            
        public int Selectable { get; set; }

        public int? WorkingGroupId { get; set; }

        public int? AdministratorId { get; set; }

        public string RelatedField { get; set; }

        public List<CaseTypeOverview> SubCaseTypes { get; set; }

    }
}