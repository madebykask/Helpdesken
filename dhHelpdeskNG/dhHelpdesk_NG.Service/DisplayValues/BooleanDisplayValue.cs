namespace DH.Helpdesk.Services.DisplayValues
{
    public sealed class BooleanDisplayValue : DisplayValue<bool>
    {
        public BooleanDisplayValue(bool value)
            : base(value)
        {
        }

        public override string GetDisplayValue()
        {
            return this.Value ? "Yes" : "No";
        }
    }
}