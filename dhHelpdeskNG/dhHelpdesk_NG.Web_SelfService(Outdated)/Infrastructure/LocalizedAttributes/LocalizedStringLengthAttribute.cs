namespace DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.SelfService.Infrastructure;

    public sealed class LocalizedStringLengthAttribute : StringLengthAttribute
    {
        public LocalizedStringLengthAttribute(int maxLength)
            : base(maxLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = string.Format(
                "string length should be between {0} and {1} character(s)",
                this.MinimumLength,
                this.MaximumLength);

            return Translation.Get(errorMessage, Enums.TranslationSource.TextTranslation);
        }
    }
}