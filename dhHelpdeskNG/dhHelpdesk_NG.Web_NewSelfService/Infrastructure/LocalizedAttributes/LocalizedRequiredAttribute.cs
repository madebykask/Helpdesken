namespace DH.Helpdesk.NewSelfService.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.NewSelfService.Infrastructure;

    public sealed class LocalizedRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("required", Enums.TranslationSource.TextTranslation);
        }
    }
}