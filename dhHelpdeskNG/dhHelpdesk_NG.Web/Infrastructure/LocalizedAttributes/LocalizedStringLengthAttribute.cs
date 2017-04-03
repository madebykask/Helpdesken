namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure;

    public sealed class LocalizedStringLengthAttribute : StringLengthAttribute
    {
        public LocalizedStringLengthAttribute(int maxLength)
            : base(maxLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = string.Format("{0} {1} {2}", Translation.GetCoreTextTranslation("Max"),
                this.MaximumLength, Translation.GetCoreTextTranslation("tecken"));

            return errorMessage;
        }
    }
}