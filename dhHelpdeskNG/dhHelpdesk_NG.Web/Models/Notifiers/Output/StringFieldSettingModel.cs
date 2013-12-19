namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    public sealed class StringFieldSettingModel : FieldSettingModel
    {
        public StringFieldSettingModel(string name, bool showInDetails, bool showInNotifiers, string caption, bool required, int? minLength, string ldapAttribute)
            : base(name, showInDetails, showInNotifiers, caption, required, ldapAttribute)
        {
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }
    }
}