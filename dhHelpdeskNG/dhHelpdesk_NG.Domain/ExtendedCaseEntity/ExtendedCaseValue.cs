using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class ExtendedCaseValueEntity : Entity
    {     
        public int ExtendedCaseDataId { get; set; }

        public string FieldId { get; set; }
        public string Value { get; set; }
        public string SecondaryValue { get; set; }

        public virtual ExtendedCaseDataEntity ExtendedCaseData { get; set; }
    }
}
