namespace dhHelpdesk_NG.Domain.Notifiers
{
    using global::System;

    public class ComputerUserFieldSettings : Entity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public string ComputerUserField { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int Customer_Id { get; set; }

        public string FieldHelp { get; set; }

        public string LDAPAttribute { get; set; }

        [Obsolete("Instead of it use ComputerUserFieldSettingsLanguage entity to get translation.")]
        public string Label { get; set; }

        [Obsolete("Never use it. Will be deleted in future.")]
        public string Label_ENG { get; set; }

        public int MinLength { get; set; }

        public int Required { get; set; }

        public int Show { get; set; }

        public int ShowInList { get; set; }

        #endregion
    }
}