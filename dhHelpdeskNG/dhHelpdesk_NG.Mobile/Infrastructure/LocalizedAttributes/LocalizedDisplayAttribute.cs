namespace DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel;

    using DH.Helpdesk.Mobile.Infrastructure;

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