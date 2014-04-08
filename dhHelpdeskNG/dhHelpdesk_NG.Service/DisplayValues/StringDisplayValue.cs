namespace DH.Helpdesk.Services.DisplayValues
{
    public sealed class StringDisplayValue : DisplayValue
    {
        private readonly string value;

        public StringDisplayValue(string value)
        {
            this.value = value;
        }

        public override string GetDisplayValue()
        {
            return this.value;
        }
    }
}