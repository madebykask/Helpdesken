﻿namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models;

    public class ConfigurableFieldModel<TValue> : IConfigurableFieldModel
    {
        #region Constructors and Destructors

        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(string caption, TValue value, bool required)
        {
            this.Show = true;
            this.Caption = caption;
            this.Value = value;
            this.IsRequired = required;
        }

        #endregion

        #region Public Properties
        public string Caption { get; set; }

        public bool IsRequired { get; set; }

        public bool Show { get; set; }

        [LocalizedRequiredFrom("IsRequired")]
        public TValue Value { get; set; }

        #endregion

        #region Public Methods and Operators

        public static ConfigurableFieldModel<TValue> CreateUnshowable()
        {
            return new ConfigurableFieldModel<TValue> { Caption = "", Show = false };
        }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }

        #endregion         
    }
}