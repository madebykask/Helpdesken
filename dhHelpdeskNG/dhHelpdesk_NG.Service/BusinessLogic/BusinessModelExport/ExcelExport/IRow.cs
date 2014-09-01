namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;

    public interface IRow<out TCell>
        where TCell : ICell
    {
        IEnumerable<TCell> Fields { get; }
    }
}