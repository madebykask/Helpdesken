using System;
using System.Collections.Generic;

namespace ExtendedCase.Dal.Data
{
    public class ExtendedCaseData : EntityBase
    {
        public int Id { get; set; }
        public int ExtendedCaseFormId { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public IList<ExtendedCaseFieldValue> FieldsValues { get; set; }
    }
}