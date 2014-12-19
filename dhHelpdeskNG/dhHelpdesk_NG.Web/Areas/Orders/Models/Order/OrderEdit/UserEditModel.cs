namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class UserEditModel
    {
        public UserEditModel()
        {            
        }

        public UserEditModel(
            ConfigurableFieldModel<string> userId,
            ConfigurableFieldModel<string> userFirstName,
            ConfigurableFieldModel<string> userLastName)
        {
            this.UserId = userId;
            this.UserFirstName = userFirstName;
            this.UserLastName = userLastName;
        }

        [NotNull]
        public ConfigurableFieldModel<string> UserId { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> UserFirstName { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> UserLastName { get; set; }

        public static UserEditModel CreateEmpty()
        {
            return new UserEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return this.UserId.Show ||
                this.UserFirstName.Show ||
                this.UserLastName.Show;
        }
    }
}