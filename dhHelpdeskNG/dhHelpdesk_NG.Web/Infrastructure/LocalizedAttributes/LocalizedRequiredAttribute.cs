namespace dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    public sealed class LocalizedRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("required", Enums.TranslationSource.TextTranslation);
        }
    }
}