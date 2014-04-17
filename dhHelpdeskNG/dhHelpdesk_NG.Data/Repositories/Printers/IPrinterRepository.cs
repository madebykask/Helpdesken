namespace DH.Helpdesk.Dal.Repositories.Printers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.Dal.Dal;

    public interface IPrinterRepository : INewRepository
    {
        void Add(Printer businessModel);

        void Update(Printer businessModel);

        Printer FindById(int id);

        List<PrinterOverview> FindOverviews(int customerId, int? departmentId, string searchFor);
    }
}