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

        private readonly IUserService userService;

        private readonly ICaseFileService caseFileService;

        private readonly IMasterDataService masterDataService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public InvoiceArticleService(
                IInvoiceArticleUnitRepository invoiceArticleUnitRepository,
                IInvoiceArticleRepository invoiceArticleRepository,
                ICaseInvoiceArticleRepository caseInvoiceArticleRepository,
                ICaseInvoiceSettingsService caseInvoiceSettingsService,
                IUserService userService,
                ICaseFileService caseFileService,
                IMasterDataService masterDataService,
                IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.invoiceArticleUnitRepository = invoiceArticleUnitRepository;
            this.invoiceArticleRepository = invoiceArticleRepository;
            this.caseInvoiceArticleRepository = caseInvoiceArticleRepository;
            this.caseInvoiceSettingsService = caseInvoiceSettingsService;
            this.userService = userService;
            this.caseFileService = caseFileService;
            this.masterDataService = masterDataService;
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
            var CaseInvoices = this.caseInvoiceArticleRepository.GetCaseInvoices(caseId);
            CaseInvoices = SetInvoicedByUsername(CaseInvoices);
            return CaseInvoices;
        }

        /// <summary>
        /// Will return caseinvoice with order.invoicedate with timezone (its saved as utc)
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="userTimeZone"></param>
        /// <returns></returns>
        public CaseInvoice[] GetCaseInvoicesWithTimeZone(int caseId, TimeZoneInfo userTimeZone)
        {
            var CaseInvoices = this.caseInvoiceArticleRepository.GetCaseInvoices(caseId);
            CaseInvoices = SetInvoicedByUsername(CaseInvoices);
            CaseInvoices = SetInvoiceOrderToTimeZone(CaseInvoices, userTimeZone);
            return CaseInvoices;
        }

        /// <summary>
        /// Will convert order.invoicedate to date with timezone (from utc)
        /// </summary>
        /// <param name="CaseInvoices"></param>
        /// <param name="userTimeZone"></param>
        /// <returns></returns>
        private CaseInvoice[] SetInvoiceOrderToTimeZone(CaseInvoice[] CaseInvoices, TimeZoneInfo TimeZone)
        {
            if (CaseInvoices != null)
            {
                if (CaseInvoices.FirstOrDefault() != null)
                {
                    foreach (var Order in CaseInvoices.FirstOrDefault().Orders)
                    {
                        if (Order.InvoiceDate != null)
                        {
                            Order.InvoiceDate = TimeZoneInfo.ConvertTimeFromUtc(Order.InvoiceDate ?? new DateTime(1970, 1, 1), TimeZone);
                        }
                    }
                }
            }
            return CaseInvoices;
        }

        private CaseInvoice[] SetInvoicedByUsername(CaseInvoice[] CaseInvoices)
        {
            if (CaseInvoices != null)
            {
                if (CaseInvoices.FirstOrDefault() != null)
                {
                    foreach (var Order in CaseInvoices.FirstOrDefault().Orders)
                    {
                        if (Order.InvoicedByUserId != null)
                        {
                            Order.InvoicedByUser = this.userService.GetUser(Order.InvoicedByUserId ?? 0).UserID;
                        }
                    }
                }
            }
            return CaseInvoices;
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
                        this.DoInvoiceXMLWork(order, customerId, CurrentUserId, caseId);
                    }
                }
            }
            this.SaveCaseInvoices(Invoices, caseId);
        }

        public bool ValidateInvoiceSettings(int customerId)
        {
            var caseInvoiceSettings = this.caseInvoiceSettingsService.GetSettings(customerId);

            if (string.IsNullOrEmpty(caseInvoiceSettings.ExportPath))
            {
                return false;
            }
            if (string.IsNullOrEmpty(caseInvoiceSettings.Currency))
            {
                return false;
            }
            if (string.IsNullOrEmpty(caseInvoiceSettings.OrderNoPrefix))
            {
                return false;
            }
            if (string.IsNullOrEmpty(caseInvoiceSettings.Issuer))
            {
                return false;
            }
            if (string.IsNullOrEmpty(caseInvoiceSettings.OurReference))
            {
                return false;
            }

            return true;
        }

        private void DoInvoiceXMLWork(CaseInvoiceOrder order, int customerId, int userId, int caseId)
        {
            var caseInvoiceSettings = this.caseInvoiceSettingsService.GetSettings(customerId);
            if (caseInvoiceSettings == null)
            {
                throw new Exception("No invoice settings for Customer");
            }
            order.DoInvoice(userId);

            var output = this.OrderToOutputXML(order, customerId, caseId);
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

        //IKEA SPECIFIC, MOVE TO OWN CLASS??
        private XmlDocument OrderToOutputXML(CaseInvoiceOrder order, int CustomerId, int CaseId)
        {
            if (order == null)
            {
                return null;
            }
            var invoiceSettings = caseInvoiceSettingsService.GetSettings(CustomerId);

            var xml = "";
            xml += OrderXMLHeader();
            xml += "<SalesDoc>";
            xml += XMLSalesHeader(order, invoiceSettings);
            xml += XMLSalesLines(order.Articles);
            xml += XMLAttachments(order.Files, CaseId, CustomerId);
            xml += "</SalesDoc>";

            using (var ms = new MemoryStream())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return xmlDoc;
            }
        }

        //IKEA XML HELPER CLASS -- MOVE
        private string XMLSalesLines(CaseInvoiceArticle[] articles)
        {
            var xml = "";
            foreach (var article in articles)
            {
                xml += XMLSalesLine(article);
            }
            return xml;
        }

        //IKEA XML HELPER CLASS -- MOVE
        private string XMLSalesLine(CaseInvoiceArticle article)
        {
            var xml = "";
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
            return xml;
        }

        //IKEA XML HELPER CLASS -- MOVE
        private string XMLSalesHeader(CaseInvoiceOrder order, CaseInvoiceSettings settings)
        {
            var xml = "";
            xml += "<SalesHeader>";
            xml += XMLRow("DocType", "valueplaceholder");
            xml += XMLRow("SellToCustomerNo", settings.Issuer);
            xml += XMLRow("OrderDate", order.InvoiceDate.Value.ToShortDateString());
            xml += XMLRow("OurReferenceName", settings.OurReference);
            xml += XMLRow("YourReferenceName", YourReferenceRow(order.CostCentre, order.Persons_Name));
            xml += XMLRow("OrderNo", OrderNoRow(settings.OrderNoPrefix, order.CaseNumber.ToString(), order.Number));
            xml += XMLRow("CurrencyCode", settings.Currency);
            //JOBNO <JobNo />??
            xml += "</SalesHeader>";
            return xml;
        }

        //IKEA XML HELPER CLASS -- MOVE
        private string XMLAttachments(CaseInvoiceOrderFile[] Files, int CaseId, int CustomerId)
        {
            string xml = "";
            if (Files.Any())
            {
                xml += "<Attachments>";
                foreach (var attachment in Files)
                {
                    xml += "<Attachment>";
                    xml += XMLRow("FileName", attachment.FileName);
                    xml += XMLRow("EncodedFile", CaseInvoiceOrderFileToBase64Encode(attachment, CaseId, CustomerId));
                    xml += "</Attachment>";
                }
                xml += "</Attachments>";
            }
            return xml;
        }

        //IKEA XML HELPER CLASS -- MOVE
        private string CaseInvoiceOrderFileToBase64Encode(CaseInvoiceOrderFile file, int caseId, int CustomerId)
        {
            byte[] fileContent;
            var basePath = string.Empty;
            basePath = masterDataService.GetFilePath(CustomerId);
            fileContent = caseFileService.GetFileContentByIdAndFileName(caseId, basePath, file.FileName);
            var encodedString = Convert.ToBase64String(fileContent);
            return encodedString;
        }

        /// <summary>
        /// sets format and assures that string is not longer than 41 characters per IKEA requirement
        /// </summary>
        /// <param name="CostCentre"></param>
        /// <param name="ReferenceName"></param>
        /// <returns></returns>
        private string YourReferenceRow(string CostCentre, string ReferenceName)
        {
            var reference = CostCentre;
            reference += "/";
            var truncatedToNLength = new string(ReferenceName.Take((41 - reference.Length)).ToArray());
            reference += truncatedToNLength;
            return reference;
        }

        /// <summary>
        /// Orderno, only for formatting it //IKEA XML HELPER CLASS -- MOVE
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="CaseNumber"></param>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        private string OrderNoRow(string Prefix, string CaseNumber, int OrderNumber)
        {
            string OrderNo = Prefix + CaseNumber + "-" + OrderNumber.ToString();
            return OrderNo;
        }

        //IKEA XML HELPER CLASS -- MOVE
        private string XMLRow(string tag, string value)
        {
            var NewXMLRow = "";
            if (string.IsNullOrEmpty(value))
            {
                NewXMLRow = "<" + tag + "/>";
            }
            else
            {
                NewXMLRow = "<" + tag + ">" + value + "</" + tag + ">";
            }
            return NewXMLRow;
        }
        //IKEA XML HELPER CLASS -- MOVE
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