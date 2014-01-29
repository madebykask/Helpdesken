namespace dhHelpdesk_NG.Data.Collections.Changes
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Domain.Changes;

    public sealed class FieldSettingCollection : List<ChangeFieldSettingsEntity>
    {
        public FieldSettingCollection(IEnumerable<ChangeFieldSettingsEntity> fieldSettings) : base(fieldSettings)
        {
            
        }

        public ChangeFieldSettingsEntity FindByName(string name)
        {
            return this.Find(s => s.ChangeField.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
