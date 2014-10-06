namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure;

    public sealed class LocalizedRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("required", Enums.TranslationSource.TextTranslation);
        }
    }
}