namespace DH.Helpdesk.Web.Models.Common
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ConfigurableSearchFieldModel<TValue>
    {
        private string caption;

        private TValue value;

        public ConfigurableSearchFieldModel(bool show)
        {
            this.Show = show;
        }

        public ConfigurableSearchFieldModel(bool show, string caption, TValue value)
            : this(show)
        {
            this.Caption = caption;
            this.Value = value;
        }

        public bool Show { get; private set; }

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
            private set
            {
                this.caption = value;
            }
        }

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
            private set
            {
                this.value = value;
            }
        }
    }
}