namespace DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel;

    using DH.Helpdesk.SelfService.Infrastructure;

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