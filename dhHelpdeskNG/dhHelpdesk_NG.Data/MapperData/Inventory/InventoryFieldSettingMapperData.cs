namespace DH.Helpdesk.Dal.MapperData.Inventory
{
    using DH.Helpdesk.Common.Collections;

    public sealed class InventoryFieldSettingMapperData : INamedObject
    {
        public string FieldName { get; set; }

        public int ShowInDetails { get; set; }

        public int ShowInList { get; set; }

        public string Caption { get; set; }

        public int PropertySize { get; set; }

        public int Position { get; set; }

        public string XMLTag { get; set; }

        public int ReadOnly { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}