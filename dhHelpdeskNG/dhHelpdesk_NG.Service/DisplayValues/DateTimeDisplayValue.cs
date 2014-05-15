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
            return this.Value.HasValue ? this.Value.Value.ToShortDateString() : null;
        }

        #endregion
    }
}