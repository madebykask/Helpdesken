using System;
using System.Collections.Generic;

namespace ExtendedCase.Models
{
    public class FormDataSaveInputModel
    {
        public Guid? UniqueId  { get; set; }
        public int HelpdeskCaseId { get; set; }
        public int FormId { get; set; }
        public IList<FieldValueModel> FieldsValues { get; set; }
        public IList<FieldValueModel> CaseFieldsValues { get; set; }
        public IList<FormStateItem> FormState { get; set; }
    }
}