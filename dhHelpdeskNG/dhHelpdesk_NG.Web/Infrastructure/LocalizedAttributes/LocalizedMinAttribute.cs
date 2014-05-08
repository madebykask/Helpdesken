namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using DataAnnotationsExtensions;

    public sealed class LocalizedMinAttribute : MinAttribute
    {
        public LocalizedMinAttribute(int min)
            : base(min)
        {
        }

        public LocalizedMinAttribute(double min)
            : base(min)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return Translation.Get("min value is: ") + this.Min;
        }
    }
}