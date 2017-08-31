namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class CaseFieldSetting : Entity, INulableCustomerEntity, IStartPageEntity
    {
        public int? Customer_Id { get; set; }
        public int FieldSize { get; set; }
        public int ListEdit { get; set; }
        public int Required { get; set; }
        public int Locked { get; set; }
        public int RequiredIfReopened { get; set; }

        /// <summary>
        /// Now used as "available" field for this customer 
        /// </summary>
        public int ShowOnStartPage { get; set; }

        public int ShowExternal { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
        //public string NameOrigin { get; set; }
        public string RelatedField { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EMailIdentifier { get; set; }
        public Guid? CaseFieldSettingsGUID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<CaseFieldSettingLanguage> CaseFieldSettingLanguages { get; set; }
    }

    public class CaseFieldSettingLanguage
    {
        public int CaseFieldSettings_Id { get; set; }
        public int Language_Id { get; set; }
        public string Label { get; set; }
        public string FieldHelp { get; set; }

        public virtual CaseFieldSetting CaseFieldSetting { get; set; }
        public virtual Language Language { get; set; }
    }

    public class CaseFieldSettingsList
    {
        public int CaseFieldSettings_Id { get; set; }
        public int? Customer_Id { get; set; }
        public string Name { get; set; }
        public int ShowOnStartPage { get; set; }
        public int ShowExternal { get; set; }
        public int Required { get; set; }
    }
}