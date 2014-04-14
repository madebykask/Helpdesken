namespace DH.Helpdesk.Dal.MapperData.Inventory
{
    using DH.Helpdesk.Common.Collections;

    public sealed class FieldOverviewSettingMapperData : INamedObject
    {
        public string FieldName { get; set; }

        public int Show { get; set; }

        public string Caption { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}
