namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.IO;
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories.Invoice;
    using DH.Helpdesk.Domain.Invoice;
    using System;

    public sealed class InvoiceArticleService : IInvoiceArticleService
    {
        private readonly IInvoiceArticleUnitRepository invoiceArticleUnitRepository;

        private readonly IInvoiceArticleRepository invoiceArticleRepository;

        private readonly ICaseInvoiceArticleRepository caseInvoiceArticleRepository;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;
               
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public InvoiceArticleService(
                IInvoiceArticleUnitRepository invoiceArticleUnitRepository, 
                IInvoiceArticleRepository invoiceArticleRepository, 
                ICaseInvoiceArticleRepository caseInvoiceArticleRepository,
                ICaseInvoiceSettingsService caseInvoiceSettingsService,
                IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.invoiceArticleUnitRepository = invoiceArticleUnitRepository;
            this.invoiceArticleRepository = invoiceArticleRepository;
            this.caseInvoiceArticleRepository = caseInvoiceArticleRepository;
            this.caseInvoiceSettingsService = caseInvoiceSettingsService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public InvoiceArticleUnit[] GetUnits(int customerId)
        {
            return this.invoiceArticleUnitRepository.GetUnits(customerId);
        }

        public InvoiceArticle[] GetArticles(int customerId, int productAreaId)
        {
            return this.invoiceArticleRepository.GetArticles(customerId, productAreaId);
        }

        public InvoiceArticle[] GetArticles(int customerId)
        {
            return this.invoiceArticleRepository.GetArticles(customerId);
        }

        public CaseInvoice[] GetCaseInvoices(int caseId)
        {
            return this.caseInvoiceArticleRepository.GetCaseInvoices(caseId);
        }

        public void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId)
        {
            this.caseInvoiceArticleRepository.SaveCaseInvoices(invoices, caseId);
        }

        public void DeleteCaseInvoices(int caseId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseInvoicesRep = uow.GetRepository<CaseInvoiceEntity>();
                var caseInvoiceOrdersRep = uow.GetRepository<CaseInvoiceOrderEntity>();
                var caseInvoiceArticlesRep = uow.GetRepository<CaseInvoiceArticleEntity>();
                var caseInvoiceOrderFilesRep = uow.GetRepository<CaseInvoiceOrderFileEntity>();

                var invoices = caseInvoicesRep.GetAll()
                                .Where(i => i.CaseId == caseId)
                                .ToList();

                foreach (var invoice in invoices)
                {
                    var orderIds = invoice.Orders.Select(o => o.Id);
                    caseInvoiceArticlesRep.DeleteWhere(a => orderIds.Contains(a.OrderId));
                    caseInvoiceOrderFilesRep.DeleteWhere(f => orderIds.Contains(f.OrderId));
                    caseInvoiceOrdersRep.DeleteWhere(o => orderIds.Contains(o.Id));
                    caseInvoicesRep.DeleteById(invoice.Id);
                }

                uow.Save();
            }
        }

        public void DoInvoiceWork(CaseInvoice[] caseInvoiceData, int caseId, int customerId, int CurrentUserId)
        {
            var Invoices = caseInvoiceData; //there will only be one?
            foreach (var order in Invoices.FirstOrDefault().Orders)
            {
                if (order.InvoicedByUserId == null)
                {
                    bool DoInvoice = false;
                    foreach (var article in order.Articles)
                    {
                        if (article.IsInvoiced)
                        {
                            DoInvoice = true;
                        }
                    }
                    if (DoInvoice)
                    {
                        this.DoInvoiceXMLWork(order, customerId, CurrentUserId);
                    }
                }
            }
            this.SaveCaseInvoices(Invoices, caseId);
        }

        private void DoInvoiceXMLWork(CaseInvoiceOrder order, int customerId, int userId)
        {
            var caseInvoiceSettings = this.caseInvoiceSettingsService.GetSettings(customerId);
            if (caseInvoiceSettings == null)
            {
                throw new Exception("No invoice settings for Customer");
            }
            order.DoInvoice(userId);

            var output = this.OrderToOutputXML(order);
            if (output == null)
            {
                throw new Exception("Couldn't create invoice-XML");
            }
            //create or check if directory exists
            if (!Directory.Exists(caseInvoiceSettings.ExportPath))
            {
                Directory.CreateDirectory(caseInvoiceSettings.ExportPath);
            }

            var path = Path.Combine(caseInvoiceSettings.ExportPath, this.GetExportFileName());
            output.Save(path);
        }

        private string GetExportFileName()
        {
            return string.Format("{0}_{1}.xml", DateTime.Now.ToShortDateString(), Guid.NewGuid());
        }

        private XmlDocument OrderToOutputXML(CaseInvoiceOrder order)
        {
            if (order == null)
            {
                return null;
            }
            var xml = "";

            xml += OrderXMLHeader();
            xml += "<SalesDoc>";
            xml += "<SalesHeader>";
            xml += XMLRow("DocType", "valueplaceholder");
            xml += XMLRow("SellToCustomerNo", "valueplaceholder");
            xml += XMLRow("OrderDate", order.InvoiceDate.Value.ToShortDateString());
            xml += XMLRow("OurReferenceName", "PLACEHOLDERVALUE");
            xml += XMLRow("YourReferenceName", order.CostCentre + "/" + order.Persons_Name);
            xml += XMLRow("OrderNo", "FA" + order.CaseNumber + "-" + order.Number);
            xml += XMLRow("CurrencyCode", "SEK");
            //JOBNO <JobNo />??

            foreach (var article in order.Articles)
            {
                xml += "<SalesLine>";
                xml += XMLRow("ItemNo", article.Article.Number);
                xml += XMLRow("Description", article.Article.Description);
                xml += XMLRow("Quantity", article.Amount.ToString());
                xml += XMLRow("UnitOfMeasureCode", article.Article.Unit.Name);
                var articlePrice = article.Ppu;
                if (articlePrice == null)
                {
                    articlePrice = article.Article.Ppu;
                }
                xml += XMLRow("UnitPrice", articlePrice.ToString());
                xml += "</SalesLine>";
            }

            xml += "</SalesHeader>";
            xml += "</SalesDoc>";
            

            using (var ms = new MemoryStream())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return xmlDoc;
            }
        }
        private string XMLRow(string tag, string value)
        {
            var NewXMLRow = "";
            NewXMLRow = "<" + tag + ">" + value + "</" + tag + ">";

            return NewXMLRow;
        }

        private string OrderXMLHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>";
        }


        public int SaveArticle(InvoiceArticle article)
        {
            return this.invoiceArticleRepository.SaveArticle(article);
        }

        public int SaveUnit(InvoiceArticleUnit unit)
        {
            return this.invoiceArticleUnitRepository.SaveUnit(unit);
        }
    }
}