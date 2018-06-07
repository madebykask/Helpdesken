namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel;

    using DH.Helpdesk.Web.Infrastructure;

    public sealed class LocalizedDisplayAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayAttribute(string displayName) : base(displayName)
        {
        }

        public override string DisplayName
        {
            get
            {
                return Translation.GetCoreTextTranslation(this.DisplayNameValue);
            }
        }
    }
}