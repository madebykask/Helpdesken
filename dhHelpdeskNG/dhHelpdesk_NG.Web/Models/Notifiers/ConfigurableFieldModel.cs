//namespace DH.Helpdesk.Web.Models.Notifiers
//{
//    using DH.Helpdesk.Common.ValidationAttributes;
//    using DH.Helpdesk.Web.LocalizedAttributes;
//
//    public sealed class ConfigurableFieldModel<TValue>
//    {
//        public ConfigurableFieldModel()
//        {
//        }
//
//        public ConfigurableFieldModel(bool show)
//        {
//            this.Show = show;
//        }
//
//        public ConfigurableFieldModel(bool show, string caption, TValue value)
//            : this(show)
//        {
//            this.Caption = caption;
//            this.Value = value;
//        }
//
//        public ConfigurableFieldModel(bool show, string caption, TValue value, bool required)
//            : this(show, caption, value)
//        {
//            this.MaxLength = 20;
//            this.Required = required;
//        }
//
//        public ConfigurableFieldModel(bool show, string caption, TValue value, bool required, int maxLength)
//            : this(show, caption, value, required)
//        {
//            this.MaxLength = maxLength;
//        }
//
//        public bool Show { get; set; }
//
//        [NotNullAndEmpty]
//        public string Caption { get; set; }
//
//        [LocalizedRequiredFrom("Required")]
//        [LocalizedMaxLengthFrom("MaxLength")]
//        public TValue Value { get; set; }
//
//        public bool Required { get; set; }
//
//        public int MaxLength { get; set; }
//
//        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
//        {
//            return field != null ? field.Value : default(TValue);
//        }
//    }
//}