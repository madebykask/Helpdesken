namespace DH.Helpdesk.Services.DisplayValues
{
    public sealed class BooleanDisplayValue : DisplayValue
    {
        private readonly bool value;

        public BooleanDisplayValue(bool value)
        {
            this.value = value;
        }

        public override string GetDisplayValue()
        {
            return this.value ? "Yes" : "No";
        }
    }
}