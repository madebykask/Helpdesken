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
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Invoice.Xml;   

    public class InvoiceArticleService : IInvoiceArticleService
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

        public int SaveArticle(InvoiceArticle article)
        {
            return this.invoiceArticleRepository.SaveArticle(article);
        }

        public int SaveUnit(InvoiceArticleUnit unit)
        {
            return this.invoiceArticleUnitRepository.SaveUnit(unit);
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

        public DataValidationResult ValidateInvoiceSettings(int customerId)
        {
            var caseInvoiceSettings = this.caseInvoiceSettingsService.GetSettings(customerId);

            if (string.IsNullOrEmpty(caseInvoiceSettings.ExportPath))
            {
                return new DataValidationResult(false, "Export Path");
            }

            if (string.IsNullOrEmpty(caseInvoiceSettings.Currency))
            {
                return new DataValidationResult(false, "Currency");
            }

            if (string.IsNullOrEmpty(caseInvoiceSettings.OrderNoPrefix))
            {
                return new DataValidationResult(false, "OrderNoPrefix");
            }

            if (string.IsNullOrEmpty(caseInvoiceSettings.Issuer))
            {
                return new DataValidationResult(false, "Issuer");
            }

            if (string.IsNullOrEmpty(caseInvoiceSettings.OurReference))
            {
                return new DataValidationResult(false, "OurReference");
            }

            return new DataValidationResult();
        }       

        public void DoInvoiceWork(CaseInvoice[] caseInvoiceData, int caseId, int customerId, int CurrentUserId, int? orderIdToXML )
        {   
         
            this.SaveCaseInvoices(caseInvoiceData, caseId);
            
            if (orderIdToXML.HasValue)
            {               
                var orderToExport = this.caseInvoiceArticleRepository.GetCaseInvoiceOrder(caseId, orderIdToXML.Value);
                if (orderToExport != null)
                {
                    var caseInvoiceSettings = this.caseInvoiceSettingsService.GetSettings(customerId);
                    if (caseInvoiceSettings == null)
                    {
                        throw new Exception("No invoice settings for Customer");
                    }
                    ExportOrder(orderToExport, caseInvoiceSettings, caseId);
                }                
            }                       
        }
        
        private void ExportOrder(CaseInvoiceOrder order, CaseInvoiceSettings caseInvoiceSettings, int caseId)
        {
            var salesDoc = MapToSalesDoc(order, caseInvoiceSettings, caseId);           
            var xmlData = salesDoc.ConvertToXML();
            if (xmlData.Key)
            {                
                if (!Directory.Exists(caseInvoiceSettings.ExportPath))
                    Directory.CreateDirectory(caseInvoiceSettings.ExportPath);
                
                var path = Path.Combine(caseInvoiceSettings.ExportPath, GetExportFileName());
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(xmlData.Value);
                tw.Close();
            }
            else
                throw new Exception("An error occurred while converting order to XML. " +  xmlData.Value);

        }

        private SalesDoc MapToSalesDoc(CaseInvoiceOrder order, CaseInvoiceSettings settings, int caseId)
        {
            var salesDoc = new SalesDoc();

            // Header
            var salesHeader = new SalesDocSalesHeader();            
            salesHeader.DocType = order.CreditForOrder_Id.HasValue ? "Credit" : "Order";
            salesHeader.SellToCustomerNo = settings.Issuer;
            salesHeader.OrderDate = order.InvoiceDate.HasValue? order.InvoiceDate.Value.ToShortDateString() : string.Empty;
            salesHeader.OurReferenceName = settings.OurReference;
            salesHeader.YourReferenceName = YourReferenceRow(order.CostCentre, order.Persons_Name);
            salesHeader.OrderNo = OrderNoRow(settings.OrderNoPrefix, order.CaseNumber.ToString(), order.Number);
            salesHeader.OurReferenceName = settings.Currency;

            // Articles
            var salesLines = new List<SalesDocSalesLine>();
            foreach (var article in order.Articles)
            {
                if (article.ArticleId.HasValue)
                    salesLines.Add(new SalesDocSalesLine() 
                    {
                        ItemNo = article.Article != null? article.Article.Number : string.Empty,
                        Description = string.Empty,
                        Quantity = article.Amount.HasValue? article.Amount.ToString() : string.Empty,
                        UnitOfMeasureCode = (article.Article != null && article.Article.Unit != null? article.Article.Unit.Name: string.Empty),
                        UnitPrice = article.Ppu.HasValue ? article.Ppu.Value.ToString() : string.Empty
                    });
                else
                    salesLines.Add(new SalesDocSalesLine()
                    {
                        ItemNo= string.Empty,
                        Description = article.Name,                                    
                        Quantity = string.Empty,
                        UnitOfMeasureCode = string.Empty,
                        UnitPrice = string.Empty
                    });
            }

            // Attachments
            var salesAttachments = new List<SalesDocAttachment>();
            foreach (var file in order.Files)
            {
                salesAttachments.Add(new SalesDocAttachment()
                {
                    FileName = file.FileName,
                    EncodedFile = CaseInvoiceOrderFileToBase64Encode(file, caseId, settings.CustomerId)
                });
            }
            
            salesDoc.SalesHeader = salesHeader;
            salesDoc.SalesLine = salesLines.ToArray();
            salesDoc.Attachments = salesAttachments.ToArray();
            return salesDoc;
        }
       
        private string YourReferenceRow(string costCentre, string referenceName)
        {
            var reference = costCentre;
            reference += "/";
            
            if (referenceName == null)
                referenceName = string.Empty;

            var truncatedToNLength = new string(referenceName.Take((41 - reference.Length)).ToArray());
            reference += truncatedToNLength;
            return reference;
        }

        private string OrderNoRow(string prefix, string caseNumber, int orderNumber)
        {
            return string.Format("{0}{1}-{2}", prefix, caseNumber, orderNumber.ToString());
        }

        private string OrderXMLHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>";
        }

        private string GetExportFileName()
        {
            return string.Format("{0}_{1}.xml", DateTime.Now.ToShortDateString(), Guid.NewGuid());
        }

        private string CaseInvoiceOrderFileToBase64Encode(CaseInvoiceOrderFile file, int caseId, int CustomerId)
        {
            byte[] fileContent;
            var basePath = string.Empty;
            basePath = masterDataService.GetFilePath(CustomerId);
            fileContent = caseFileService.GetFileContentByIdAndFileName(caseId, basePath, file.FileName);
            var encodedString = Convert.ToBase64String(fileContent);
            return encodedString;
        }
              
    }
}