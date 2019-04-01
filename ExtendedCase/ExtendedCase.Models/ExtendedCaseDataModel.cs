using System;
using System.Collections.Generic;

namespace ExtendedCase.Models
{
    public class ExtendedCaseDataModel
    {
        #region ctor()

        public ExtendedCaseDataModel()
        {
        }

        public ExtendedCaseDataModel(int id, Guid extendedCaseGuid, int extendedCaseFormId)
            : this(id, extendedCaseGuid, extendedCaseFormId, null, null)
        {
        }

        public ExtendedCaseDataModel(int id, Guid extendedCaseGuid, int extendedCaseFormId, CaseData data, IList<FormStateItem> formState)
        {
            Id = id;
            ExtendedCaseGuid = extendedCaseGuid;
            ExtendedCaseFormId = extendedCaseFormId;
            Data = data;
            FormState = formState;
        }

        #endregion

        public int Id { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public int ExtendedCaseFormId  { get; set; }
        public CaseData Data { get; set; }
        public IList<FormStateItem> FormState { get; set; }
    }

    public class CaseData
    {
        public IList<FieldValueModel> CaseFieldsValues { get; set; }
        public IList<FieldValueModel> ExtendedCaseFieldsValues { get; set; }
    }

    public class FieldValueModel
    {
        public FieldValueModel()
        {
        }

        public FieldValueModel(string fieldId, string value, string secondaryValue, FieldProperties properties)
        {
            FieldId = fieldId;
            Value = value;
            SecondaryValue = secondaryValue;
            Properties = properties;
        }

        public string FieldId { get; set; }
        public string Value { get; set; }
        public string SecondaryValue { get; set; }
        public FieldProperties Properties { get; set; }
    }

    public class FieldProperties
    {
        public FieldProperties()
        {
        }

        public bool Pristine;
    }

    public class FormStateItem
    {
        public string TabId { get; set; }
        public string SectionId { get; set; }
        public int SectionIndex { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

