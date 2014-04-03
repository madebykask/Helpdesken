namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralViewModel
    {
        #region Constructors and Destructors

        public GeneralViewModel()
        {
        }

        public GeneralViewModel(
            ConfigurableFieldModel<SelectList> statuses,
            ConfigurableFieldModel<SelectList> systems,
            ConfigurableFieldModel<SelectList> objects,
            ConfigurableFieldModel<SelectList> workingGroups,
            ConfigurableFieldModel<SelectList> administrators,
            GeneralModel general)
        {
            this.Statuses = statuses;
            this.Systems = systems;
            this.Objects = objects;
            this.WorkingGroups = workingGroups;
            this.Administrators = administrators;
            this.General = general;
        }

        #endregion

        #region Public Properties

        [NotNull]
        public ConfigurableFieldModel<SelectList> Administrators { get; private set; }

        [NotNull]
        public GeneralModel General { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Objects { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Statuses { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Systems { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> WorkingGroups { get; private set; }

        #endregion
    }
}