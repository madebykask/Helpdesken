using System;

namespace DH.Helpdesk.Domain
{
    public class CaseSolutionConditionPropertyEntity : Entity
    {
        public int Id { get; set; }
        public string CaseSolutionConditionProperty { get; set; }
        public string Text { get; set; }

        public string Table { get; set; }
        public string TableFieldId { get; set; }
        public string TableFieldName { get; set; }
        public string TableFieldGuid { get; set; }
        public int SortOrder { get; set; }
        public int Status { get; set; }
    }
}
