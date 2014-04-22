namespace DH.Helpdesk.Services.DisplayValues
{
    using DH.Helpdesk.Common.Types;

    public sealed class UserNameDisplayValue : DisplayValue<UserName>
    {
        public UserNameDisplayValue(UserName value)
            : base(value)
        {
        }

        public static explicit operator UserNameDisplayValue(UserName value)
        {
            var displayValue = new UserNameDisplayValue(value);

            return displayValue;
        }

        public override string GetDisplayValue()
        {
            if (this.Value == null)
            {
                return null;
            }

            return this.Value.FirstName + " " + this.Value.LastName;
        }
    }
}