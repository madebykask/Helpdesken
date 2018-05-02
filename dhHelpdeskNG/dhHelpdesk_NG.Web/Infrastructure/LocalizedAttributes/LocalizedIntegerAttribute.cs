namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using DataAnnotationsExtensions;

    public sealed class LocalizedIntegerAttribute : IntegerAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.GetCoreTextTranslation("måste vara ett heltal");
        }
    }
}