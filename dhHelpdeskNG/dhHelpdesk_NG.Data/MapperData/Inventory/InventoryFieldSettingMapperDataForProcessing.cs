namespace DH.Helpdesk.Dal.MapperData.Inventory
{
    using DH.Helpdesk.Common.Collections;

    public sealed class InventoryFieldSettingMapperDataForProcessing : INamedObject
    {
        public string FieldName { get; set; }

        public int Show { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}