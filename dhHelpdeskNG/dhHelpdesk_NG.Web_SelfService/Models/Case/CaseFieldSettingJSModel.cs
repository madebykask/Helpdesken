using System.Collections.Generic;

namespace DH.Helpdesk.SelfService.Models.Case
{
   
    public sealed class FieldSettingJSModel
    {
        public FieldSettingJSModel(string fieldName, bool isVisible, bool isReadonly, bool isRequired)
        {
            FieldName = fieldName;
            IsVisible = isVisible;
            IsReadonly = isReadonly;
            IsRequired = isRequired;
        }

        public string FieldName { get; private set; }

        public bool IsVisible { get; private set; }

        public bool IsReadonly { get; private set; }

        public bool IsRequired { get; private set; }

    }

}
