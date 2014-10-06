namespace DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes
{
    using DataAnnotationsExtensions;

    public sealed class LocalizedIntegerAttribute : IntegerAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("must be an integer.");
        }
    }
}