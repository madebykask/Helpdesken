using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.SelfService.Models.Orders.FieldModels
{
    public sealed class ConfigurableFieldModel<TValue> : IConfigurableFieldModel
    {
        #region Constructors and Destructors

        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(string caption, TValue value, bool required, string help)
        {
            Show = true;
            Caption = caption;
            Value = value;
            IsRequired = required;
            Help = help;
        }

        #endregion

        #region Public Properties

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public bool IsRequired { get; set; }

        public bool Show { get; set; }

        [LocalizedRequiredFrom("IsRequired")]
        public TValue Value { get; set; }

        public string Help { get; set; }

        #endregion

        #region Public Methods and Operators

        public static ConfigurableFieldModel<TValue> CreateUnshowable()
        {
            return new ConfigurableFieldModel<TValue> { Show = false };
        }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }

        public static List<int> GetValueOrDefault(ConfigurableFieldModel<List<CheckBoxListItem>> field)
        {
            return field?.Value?.Where(s => s.IsChecked).Select(s => s.Id).ToList() ?? new List<int>();
        }

        public static string GetValueOrDefault(ConfigurableFieldModel<string> field)
        {
            return field?.Value ?? string.Empty;
        }

        #endregion         
    }
}