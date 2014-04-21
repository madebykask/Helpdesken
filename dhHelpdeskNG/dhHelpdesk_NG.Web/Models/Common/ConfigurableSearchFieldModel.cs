namespace DH.Helpdesk.Web.Models.Common
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ConfigurableSearchFieldModel<TValue>
    {
        #region Fields

        private string caption;

        private TValue value;

        #endregion

        #region Constructors and Destructors

        public ConfigurableSearchFieldModel(string caption, TValue value)
        {
            this.Show = true;
            this.Caption = caption;
            this.Value = value;
        }

        private ConfigurableSearchFieldModel()
        {
        }

        #endregion

        #region Public Properties

        [NotNullAndEmpty]
        public string Caption
        {
            get
            {
                if (this.Show)
                {
                    return this.caption;
                }

                throw new InvalidOperationException("Cannot provide \"Caption\" for unshowable field.");
            }

            set
            {
                this.caption = value;
            }
        }

        public bool Show { get; set; }

        public TValue Value
        {
            get
            {
                if (this.Show)
                {
                    return this.value;
                }

                throw new InvalidOperationException("Cannot provide \"Value\" for unshowable field.");
            }

            set
            {
                this.value = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static ConfigurableSearchFieldModel<TValue> CreateUnshowable()
        {
            return new ConfigurableSearchFieldModel<TValue> { Show = false };
        }

        #endregion
    }
}