namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting : BusinessModel
    {
        private readonly ModelStates modelStates;

        private FieldSetting(
            ModelStates modelStates,
            string name,
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired,
            bool isReadOnly)
        {
            this.modelStates = modelStates;
            this.Name = name;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }

        [AllowRead(ModelStates.Created | ModelStates.ForEdit)]
        public DateTime ChangedDate { get; private set; }

        public static FieldSetting CreateForUpdate(
            string name,
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired,
            bool isReadOnly,
            DateTime changedDate)
        {
            var model = new FieldSetting(ModelStates.Updated, name, showInDetails, showInList, caption, isRequired, isReadOnly) { ChangedDate = changedDate };

            return model;
        }

        public static FieldSetting CreateForEdit(
            string name,
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired,
            bool isReadOnly)
        {
            var model = new FieldSetting(ModelStates.Created, name, showInDetails, showInList, caption, isRequired, isReadOnly);

            return model;
        }
    }
}
