namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel;

    public sealed class LocalizedDisplayAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayAttribute(string displayName) : base(displayName)
        {
        }

        public override string DisplayName
        {
            get
            {
                return Translation.Get(this.DisplayNameValue, Enums.TranslationSource.TextTranslation);
            }
        }
    }
}