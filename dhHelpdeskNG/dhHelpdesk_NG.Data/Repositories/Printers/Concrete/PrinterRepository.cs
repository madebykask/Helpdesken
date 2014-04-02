namespace DH.Helpdesk.Dal.Repositories.Printers.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class PrinterRepository : Repository<Domain.Printers.PrinterFieldSettings>, IPrinterRepository
    {
        public PrinterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(Printer businessModel)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Printer businessModel)
        {
            throw new System.NotImplementedException();
        }

        public Printer FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<PrinterOverview> FindOverviews(int customerId, string searchFor)
        {
            throw new System.NotImplementedException();
        }
    }
}
