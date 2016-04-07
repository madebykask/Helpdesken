namespace DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.SelfService.Infrastructure;

    public sealed class LocalizedRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("required", Enums.TranslationSource.TextTranslation);
        }
    }
}