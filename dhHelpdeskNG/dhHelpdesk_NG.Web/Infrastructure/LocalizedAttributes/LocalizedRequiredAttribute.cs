namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure;

    public sealed class LocalizedRequiredAttribute : RequiredAttribute
    {
        private readonly string _text;

        public LocalizedRequiredAttribute() { }

        public LocalizedRequiredAttribute(string text)
        {
            _text = text;
        }

        public override string FormatErrorMessage(string name)
        {
            return Translation.GetCoreTextTranslation(string.IsNullOrWhiteSpace(_text) ? "fältet är obligatoriskt" : _text);
        }
    }


}