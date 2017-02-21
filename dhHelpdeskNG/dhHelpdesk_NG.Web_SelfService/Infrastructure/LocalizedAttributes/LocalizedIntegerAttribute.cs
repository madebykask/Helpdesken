using DataAnnotationsExtensions;

namespace DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes
{
    public sealed class LocalizedIntegerAttribute : IntegerAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("must be an integer.");
        }
    }
}