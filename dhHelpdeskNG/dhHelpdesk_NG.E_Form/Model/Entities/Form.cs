using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Model.Entities
{
    public class Form
    {
        public int Id { get; set; }
        public Guid FormGuid { get; set; }
        public string FormName { get; set; }
        public string FormPath { get; set; }
        public string FormXmlPath { get; set; }
        public int CustomerId { get; set; }
        public int? ProductAreaId { get; set; }
        public int? CaseTypeId { get; set; }
        public int? PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int? CategoryId { get; set; }
        public int? StateSecondaryId { get; set; }
        public int? WorkingGroupId { get; set; }
        public int SolutionTime { get; set; }

        //public int TextTypeId { get; set; }
        //public bool LogTranslations { get; set; }
        //public string DateFormat { get; set; }

        public FormSettings FormSettings { get; set; }

    }
}
