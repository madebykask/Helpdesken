namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class BusinessItem : IRow<BusinessItemField>
    {
        public BusinessItem(IEnumerable<BusinessItemField> fields)
        {
            this.Fields = fields;
        }

        [NotNull]
        public IEnumerable<BusinessItemField> Fields { get; private set; }
    }
}