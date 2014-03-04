namespace DH.Helpdesk.Dal.MapperData.Changes
{
    using DH.Helpdesk.Common.Collections;

    public sealed class FieldProcessingSettingMapperData : INamedObject
    {
        public string ChangeField { get; set; }

        public int Show { get; set; }

        public int Required { get; set; }

        public string GetName()
        {
            return this.ChangeField;
        }
    }
}
