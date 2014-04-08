namespace DH.Helpdesk.Services.DisplayValues
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Common.Types;

    public sealed class UserNameDisplayValue : DisplayValue<UserName>
    {
        public UserNameDisplayValue(UserName value)
            : base(value)
        {
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