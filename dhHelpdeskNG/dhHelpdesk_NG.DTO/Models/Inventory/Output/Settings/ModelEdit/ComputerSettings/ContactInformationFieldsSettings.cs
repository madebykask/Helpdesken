namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContactInformationFieldsSettings
    {
        public ContactInformationFieldsSettings(
            ModelEditFieldSetting userIdFieldSetting, 
            ModelEditFieldSetting firstNameFieldSetting,
            ModelEditFieldSetting lastNameFieldSetting, 
            ModelEditFieldSetting departmentFieldSetting,
            ModelEditFieldSetting unitFieldSetting)
        {
            UserIdFieldSetting = userIdFieldSetting;
            FirstNameFieldSetting = firstNameFieldSetting;
            LastNameFieldSetting = lastNameFieldSetting;
            DepartmentFieldSetting = departmentFieldSetting;
            UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting UserIdFieldSetting { get; set; }

        public ModelEditFieldSetting FirstNameFieldSetting { get; set; }

        public ModelEditFieldSetting LastNameFieldSetting { get; set; }

        public ModelEditFieldSetting DepartmentFieldSetting { get; set; }

        public ModelEditFieldSetting UnitFieldSetting { get; set; }

    }
}