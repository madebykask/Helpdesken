namespace DH.Helpdesk.Mobile.Models.Shared
{
    public sealed class ConfigurableContainerModel<TContainer>
    {
        #region Constructors and Destructors

        public ConfigurableContainerModel()
        {
        }

        public ConfigurableContainerModel(string caption, TContainer container)
        {
            this.Show = true;
            this.Caption = caption;
            this.Container = container;
        }

        #endregion

        #region Public Properties

        public string Caption { get; set; }

        public TContainer Container { get; set; }

        public bool Show { get; set; }

        #endregion

        #region Public Methods and Operators

        public static ConfigurableContainerModel<TContainer> CreateUnshowable()
        {
            return new ConfigurableContainerModel<TContainer> { Show = false };
        }

        #endregion
    }
}