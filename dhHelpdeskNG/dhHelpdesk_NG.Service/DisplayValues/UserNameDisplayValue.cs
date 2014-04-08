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