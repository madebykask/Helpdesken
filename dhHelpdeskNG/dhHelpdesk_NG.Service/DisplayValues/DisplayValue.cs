namespace DH.Helpdesk.Services.DisplayValues
{
    public abstract class DisplayValue<TValue>
    {
        protected DisplayValue(TValue value)
        {
            this.Value = value;
        }

        protected TValue Value;

        public abstract string GetDisplayValue();
    }
}