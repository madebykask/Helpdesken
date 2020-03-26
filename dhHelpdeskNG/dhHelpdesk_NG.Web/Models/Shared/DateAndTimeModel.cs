namespace DH.Helpdesk.Web.Models.Shared
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class DateAndTimeModel
    {
        #region Constructors and Destructors

        public DateAndTimeModel()
        {
        }

        public DateAndTimeModel(ConfigurableFieldModel<DateTime?> date, ConfigurableFieldModel<string> time)
        {
            this.Date = date;
            this.Time = time;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<DateTime?> Date { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Time { get; set; }

        #endregion
    }
}