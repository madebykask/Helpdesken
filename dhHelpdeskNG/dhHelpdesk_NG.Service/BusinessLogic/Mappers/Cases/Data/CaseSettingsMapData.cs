namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Cases.Data
{
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;

    internal sealed class CaseSettingsMapData : INamedObject
    {
        public string FieldName { get; set; }

        public int Show { get; set; }

        public int ShowInList { get; set; }

        public string Caption { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }

        public string GetFieldCaption()
        {
            if (string.IsNullOrEmpty(this.Caption))
            {
                return this.FieldName;
            }

            return this.Caption;
        }

        public bool IsShow()
        {
            return this.Show.ToBool();
        }

        public bool IsShowInList()
        {
            return this.Show.ToBool() &&
                this.ShowInList.ToBool();
        }
    }
}