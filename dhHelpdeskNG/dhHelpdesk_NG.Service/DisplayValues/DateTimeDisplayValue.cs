namespace DH.Helpdesk.Services.DisplayValues
{
    using System;

    public sealed class DateTimeDisplayValue : DisplayValue<DateTime?>
    {
        #region Constructors and Destructors

        public DateTimeDisplayValue(DateTime? value)
            : base(value)
        {
        }

        #endregion

        #region Public Methods and Operators

        public static explicit operator DateTimeDisplayValue(DateTime? value)
        {
            return new DateTimeDisplayValue(value);
        }

        public override string GetDisplayValue()
        {
            if (this.Value.HasValue)
            {
                var old = this.Value;
                var newv = DateTime.SpecifyKind(this.Value.Value, DateTimeKind.Utc).ToLocalTime();
                return newv.ToShortDateString();
            }

            return null;
        }

        #endregion
    }
}