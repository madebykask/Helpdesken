namespace DH.Helpdesk.Dal.MapperData.Inventory
{
    using DH.Helpdesk.Common.Collections;

    public sealed class FieldSettingMapperData : INamedObject
    {
        public string FieldName { get; set; }

        public string Caption { get; set; }

        public int ShowInDetails { get; set; }

        public int ShowInList { get; set; }

        public int Required { get; set; }

        public int ReadOnly { get; set; }

        public int Copy { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}
