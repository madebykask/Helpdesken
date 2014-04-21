namespace DH.Helpdesk.Services.DisplayValues
{
    using DH.Helpdesk.Common.Types;

    public sealed class UserNameDisplayValue : DisplayValue
    {
        private readonly UserName value;

        public UserNameDisplayValue(UserName value)
        {
            this.value = value;
        }

        public static explicit operator UserNameDisplayValue(UserName value)
        {
            var displayValue = new UserNameDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            if (this.value == null)
            {
                return null;
            }

            return this.value.FirstName + " " + this.value.LastName;
        }
    }
}