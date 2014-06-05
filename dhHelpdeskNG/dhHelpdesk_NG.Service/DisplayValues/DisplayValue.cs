namespace DH.Helpdesk.Services.DisplayValues
{
    public abstract class DisplayValue
    {
        public abstract string GetDisplayValue();
    }

    public abstract class DisplayValue<T> : DisplayValue
    {
        protected DisplayValue(T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }
    }
}