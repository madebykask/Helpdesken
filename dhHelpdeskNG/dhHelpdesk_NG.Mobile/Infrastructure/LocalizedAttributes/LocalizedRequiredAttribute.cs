namespace DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Mobile.Infrastructure;

    public sealed class LocalizedRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("required", Enums.TranslationSource.TextTranslation);
        }
    }
}