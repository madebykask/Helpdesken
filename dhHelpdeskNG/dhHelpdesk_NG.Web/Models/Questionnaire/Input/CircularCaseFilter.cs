using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.Questionnaire.Input
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

        [DataType(DataType.Date)]
        public DateTime? FinishingDateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishingDateTo { get; set; }

        public bool IsUniqueEmail { get; set; }

        public IList<int> SelectedDepartments { get; set; }

        public IList<int> SelectedCaseTypes { get; set; }

        public IList<int> SelectedProductAreas { get; set; }

        public IList<int> SelectedWorkingGroups { get; set; }

        public int SelectedProcent { get; set; }
    }
}