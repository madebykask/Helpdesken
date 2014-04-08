namespace DH.Helpdesk.Services.DisplayValues
{
    public sealed class StringDisplayValue : DisplayValue<string>
    {
        public StringDisplayValue(string value)
            : base(value)
        {
        }

        public override string GetDisplayValue()
        {
            return this.Value;
        }
    }
}