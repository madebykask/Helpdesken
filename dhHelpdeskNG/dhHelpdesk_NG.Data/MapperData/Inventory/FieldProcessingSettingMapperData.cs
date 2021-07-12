namespace DH.Helpdesk.Dal.MapperData.Inventory
{
    using DH.Helpdesk.Common.Collections;

    public sealed class FieldProcessingSettingMapperData : INamedObject
    {
        public string FieldName { get; set; }

        public int Show { get; set; }

        public int ReadOnly { get; set; }

        public int Copy { get; set; }

        public int Required { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}