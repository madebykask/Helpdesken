namespace DH.Helpdesk.Dal.MapperData.Changes
{
    using DH.Helpdesk.Common.Collections;

    public sealed class FieldEditSettingMapperData : INamedObject
    {
        public string ChangeField { get; set; }

        public int Show { get; set; }

        public string Caption { get; set; }

        public int Required { get; set; }

        public string InitialValue { get; set; }

        public string Bookmark { get; set; }

        public string GetName()
        {
            return this.ChangeField;
        }
    }
}