namespace DH.Helpdesk.Dal.Repositories.Printers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.Dal.Dal;

    public interface IPrinterRepository : INewRepository
    {
        void Add(PrinterForInsert businessModel);

        void Update(PrinterForUpdate businessModel);

        void DeleteById(int id);

        PrinterForRead FindById(int id);

        List<PrinterOverview> FindOverviews(int customerId, int? departmentId, string searchFor, int? recordCount);

        int GetPrinterCount(int customerId, int? departmentId);
        int GetIdByName(string printerName, int customerId);
    }
}