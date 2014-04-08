namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class BusinessItem
    {
        public BusinessItem(List<BusinessItemField> fields)
        {
            this.Fields = fields;
        }

        [NotNull]
        public List<BusinessItemField> Fields { get; private set; }
    }
}