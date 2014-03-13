namespace DH.Helpdesk.Web.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure;

    public sealed class LocalizedStringLengthAttribute : StringLengthAttribute
    {
        public LocalizedStringLengthAttribute(int maximumLength)
            : base(maximumLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            var message = string.Format(
                "string length should be between {0} and {1} character(s)", this.MinimumLength, this.MaximumLength);

            return Translation.Get(message, Enums.TranslationSource.TextTranslation);
        }
    }
}