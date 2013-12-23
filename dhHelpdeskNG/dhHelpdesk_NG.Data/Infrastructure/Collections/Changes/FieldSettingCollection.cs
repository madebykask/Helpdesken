namespace dhHelpdesk_NG.Data.Infrastructure.Collections.Changes
{
    using System;
    using System.Collections.Generic;
    using dhHelpdesk_NG.Domain;

    public sealed class FieldSettingCollection : List<ChangeFieldSettings>
    {
        public FieldSettingCollection(IEnumerable<ChangeFieldSettings> fieldSettings) : base(fieldSettings)
        {
            
        }

        public ChangeFieldSettings FindByName(string name)
        {
            return this.Find(s => s.ChangeField.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
