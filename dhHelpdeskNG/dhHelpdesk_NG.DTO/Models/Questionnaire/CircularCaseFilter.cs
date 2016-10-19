using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Questionnaire
{
    public class CircularCaseFilter
    {
        public CircularCaseFilter()
        {
            SelectedDepartments = new List<int>();
            SelectedCaseTypes = new List<int>();
            SelectedProductAreas = new List<int>();
            SelectedWorkingGroups = new List<int>();
        }

        public DateTime? FinishingDateFrom { get; set; }

        public DateTime? FinishingDateTo { get; set; }

        public bool IsUniqueEmail { get; set; }

        public IList<int> SelectedDepartments { get; set; }

        public IList<int> SelectedCaseTypes { get; set; }

        public IList<int> SelectedProductAreas { get; set; }

        public IList<int> SelectedWorkingGroups { get; set; }

        public int SelectedProcent { get; set; }
    }
}
