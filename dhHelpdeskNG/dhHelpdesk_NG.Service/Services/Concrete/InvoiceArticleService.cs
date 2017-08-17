﻿namespace DH.Helpdesk.Services.Services.Concrete
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
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.Extensions.String;

    public class InvoiceArticleService : IInvoiceArticleService
    {
        private readonly IInvoiceArticleUnitRepository invoiceArticleUnitRepository;

        private readonly IInvoiceArticleRepository invoiceArticleRepository;

        private readonly ICaseInvoiceArticleRepository caseInvoiceArticleRepository;

        private readonly ICaseInvoiceSettingsService caseInvoiceSettingsService;

        private readonly IProjectService projectService;

        private readonly IUserService userService;

        private readonly ICaseFileService caseFileService;

        private readonly IDepartmentService departmentService;

        private readonly IMasterDataService masterDataService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public InvoiceArticleService(
                IInvoiceArticleUnitRepository invoiceArticleUnitRepository,
                IInvoiceArticleRepository invoiceArticleRepository,
                ICaseInvoiceArticleRepository caseInvoiceArticleRepository,
                ICaseInvoiceSettingsService caseInvoiceSettingsService,
                IUserService userService,
                IProjectService projectService,
                ICaseFileService caseFileService,
                IDepartmentService departmentService,
                IMasterDataService masterDataService,
                IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.invoiceArticleUnitRepository = invoiceArticleUnitRepository;
            this.invoiceArticleRepository = invoiceArticleRepository;
            this.caseInvoiceArticleRepository = caseInvoiceArticleRepository;
            this.caseInvoiceSettingsService = caseInvoiceSettingsService;
            this.userService = userService;
            this.projectService = projectService;
            this.caseFileService = caseFileService;
            this.departmentService = departmentService;
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

		public List<InvoiceArticle> GetActiveArticles(int customerId)
		{
			return this.invoiceArticleRepository.GetActiveArticles(customerId);
		}

		public CaseInvoice[] GetCaseInvoices(int caseId)
        {
            var CaseInvoices = this.caseInvoiceArticleRepository.GetCaseInvoices(caseId);
            CaseInvoices = SetInvoicedByUsername(CaseInvoices);
            return CaseInvoices;
        }

        public CaseInvoiceOrder[] GetInvoiceOrders(int caseId, InvoiceOrderFetchStatus status)
        {
            return this.caseInvoiceArticleRepository.GetOrders(caseId, status);
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

        public void SaveArticleProductArea(InvoiceArticleProductAreaSelectedFilter selectedItems)
        {
            this.invoiceArticleRepository.SaveArticleProductArea(selectedItems);
        }

        public void DeleteArticleProductArea(int articleid, int productareaid)
        {
            this.invoiceArticleRepository.DeleteArticleProductArea(articleid, productareaid);
        }

        public void DeactivateArticlesBySyncDate(int customerId, DateTime lastSyncDate)
        {
            invoiceArticleRepository.DeactivateArticlesBySyncDate(customerId, lastSyncDate);
        }

        public void SaveArticles(List<InvoiceArticle> articles)
        {
            invoiceArticleRepository.SaveArticles(articles);
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
                    var orders = CaseInvoices.FirstOrDefault().Orders;
                    if (orders != null)
                    {
                        foreach (var Order in orders)                           
                        {
                            if (Order.InvoiceDate != null)
                            {
                                Order.InvoiceDate = TimeZoneInfo.ConvertTimeFromUtc(Order.InvoiceDate ?? new DateTime(1970, 1, 1), TimeZone);
                            }
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
                    var orders = CaseInvoices.FirstOrDefault().Orders;
                    if (orders != null)
                    {
                        foreach (var Order in orders)
                        {
                            if (Order.InvoicedByUserId != null)
                            {
                                Order.InvoicedByUser = this.userService.GetUser(Order.InvoicedByUserId ?? 0).UserID;
                            }
                        }
                    }
                }
            }
            return CaseInvoices;
        }

        public int SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId, int userId)
        {
            return this.caseInvoiceArticleRepository.SaveCaseInvoices(invoices, caseId, userId);
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

            if (string.IsNullOrEmpty(caseInvoiceSettings.Issuer))
            {
                return new DataValidationResult(false, "Issuer");
            }

            if (string.IsNullOrEmpty(caseInvoiceSettings.OurReference))
            {
                return new DataValidationResult(false, "OurReference");
            }

            if (string.IsNullOrEmpty(caseInvoiceSettings.DocTemplate))
            {
                return new DataValidationResult(false, "DocTemplate");
            }

            return new DataValidationResult();
        }

        public ProcessResult DoInvoiceWork(CaseInvoice[] caseInvoiceData, int caseId, decimal caseNumber, int customerId, int userId, int? orderIdToXML)
        {            

            var newOrderId = this.SaveCaseInvoices(caseInvoiceData, caseId, userId);
            
            if (orderIdToXML.HasValue)
            {   
                // It means user has pressed Send button directly before save the order
                if (orderIdToXML <= 0)
                    orderIdToXML = newOrderId;

                var orderToExport = this.caseInvoiceArticleRepository.GetCaseInvoiceOrder(caseId, orderIdToXML.Value);
                if (orderToExport != null)
                {
                    var caseInvoiceSettings = this.caseInvoiceSettingsService.GetSettings(customerId);
                    if (caseInvoiceSettings == null)
                        return new ProcessResult(System.Reflection.MethodBase.GetCurrentMethod().Name, 
                                                 ProcessResult.ResultTypeEnum.ERROR, "There is no invoice settings for Customer");
                    
                    var res = ExportOrder(orderToExport, caseInvoiceSettings, caseId, caseNumber);
                    if (!res.IsSucceed)
                    {
                        this.caseInvoiceArticleRepository.CancelInvoiced(caseId, orderIdToXML.Value);
                        return new ProcessResult(res.ProcessName, ProcessResult.ResultTypeEnum.WARNING, res.LastMessage);
                    }
                }                
            }

            return new ProcessResult(System.Reflection.MethodBase.GetCurrentMethod().Name);
           
        }

        public void DeleteFileByCaseId(int caseId, string fileName)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseInvoiceOrderFilesRep = uow.GetRepository<CaseInvoiceOrderFileEntity>();
                var recToDelete = caseInvoiceOrderFilesRep.Find(f => f.Order.Invoice.CaseId == caseId && f.FileName == fileName).Select(f=> f.Id).ToList();
                if (recToDelete.Any())
                {
                    caseInvoiceOrderFilesRep.DeleteWhere(f => recToDelete.Contains(f.Id));
                    uow.Save();
                }
            }
        }

        private ProcessResult ExportOrder(CaseInvoiceOrder order, CaseInvoiceSettings caseInvoiceSettings, int caseId, decimal caseNumber)
        {            
            try
            {                    
                var salesDoc = MapToSalesDoc(order, caseInvoiceSettings, caseId, caseNumber);
                var xmlData = salesDoc.ConvertToXML();
                if (xmlData.IsSucceed)
                {
                    try
                    {
                        if (!Directory.Exists(caseInvoiceSettings.ExportPath))
                            Directory.CreateDirectory(caseInvoiceSettings.ExportPath);

                        var path = Path.Combine(caseInvoiceSettings.ExportPath, GetExportFileName());
                        TextWriter tw = new StreamWriter(path, true);
                        tw.WriteLine(xmlData.Data.ToString());
                        tw.Close();
                        return new ProcessResult(xmlData.ProcessName);
                    }
                    catch (Exception ex1)
                    {
                        return new ProcessResult(xmlData.ProcessName, ProcessResult.ResultTypeEnum.ERROR, "An error occurred while saving XML file. <br/>" + ex1.Message);
                    }
                }
                else
                    return new ProcessResult(xmlData.ProcessName, ProcessResult.ResultTypeEnum.ERROR, "An error occurred while converting order to XML.<br/>" + xmlData.LastMessage);
            }
            catch (Exception ex)
            {
                return new ProcessResult(System.Reflection.MethodBase.GetCurrentMethod().Name, ProcessResult.ResultTypeEnum.ERROR, ex.Message);
            }            
        }

        private SalesDoc MapToSalesDoc(CaseInvoiceOrder order, CaseInvoiceSettings settings,
                                       int caseId, decimal caseNumber)
        {            
            var salesDoc = new SalesDoc();

            #region Invoice data

            var curOrderSeq = GetSequenceNumber(caseId, order); 
            var originalOrderSeq = 0;
            if (order.CreditForOrder_Id.HasValue)
            {
                var originalOrder = this.caseInvoiceArticleRepository.GetCaseInvoiceOrder(caseId, order.CreditForOrder_Id.Value);
                if (originalOrder != null)
                    originalOrderSeq = GetSequenceNumber(caseId, originalOrder);
            }

            var salesHeader = new SalesDocSalesHeader();
            salesHeader.CompanyNo = settings.Issuer;
            salesHeader.DocTemplate = settings.DocTemplate;
            salesHeader.DocType = order.CreditForOrder_Id.HasValue ? InvoiceXMLDocType.Credit : InvoiceXMLDocType.Order;
            salesHeader.SellToCustomerNo = GetSellToCustomerNo(order.Department_Id);
            salesHeader.Date = order.InvoiceDate.HasValue ? order.InvoiceDate.Value.ToShortDateString() : string.Empty;
            salesHeader.DueDate = order.InvoiceDate.HasValue ? order.InvoiceDate.Value.ToShortDateString() : string.Empty;
            salesHeader.OurReference = settings.OurReference.QautationFix();
            salesHeader.YourReference2 = YourReferenceRow(order.CostCentre, order.Persons_Name.QautationFix());
            salesHeader.OrderNo = OrderNoRow(caseNumber, settings.OrderNoPrefix, curOrderSeq, originalOrderSeq);
            salesHeader.CurrencyCode = settings.Currency;
            salesHeader.JobNo = order.Project_Id.HasValue? GetJobNo(order.Project_Id.Value) : string.Empty;

            #endregion

            #region Article and Lines

            var salesLines = new List<SalesDocSalesHeaderSalesLine>();
            int lineNo = 0;
            foreach (var article in order.Articles)
            {
                lineNo++;                
                if (article.ArticleId.HasValue)
                {        
                    
                    var amountStr = article.Amount.HasValue? article.Amount.Value.ToString() : string.Empty;
                    var ppuStr = article.Ppu.HasValue ? article.Ppu.Value.ToString() : string.Empty;

                    var detectedDecimalSep = DetectDecimalSeparator(ppuStr);
                    if (string.IsNullOrEmpty(detectedDecimalSep))
                        detectedDecimalSep = DetectDecimalSeparator(amountStr);

                    salesLines.Add(new SalesDocSalesHeaderSalesLine()
                    {
                        LineNo = lineNo.ToString(),
                        LineType = InvoiceXMLLineType.Article,
                        Number = article.Article != null ? article.Article.Number : string.Empty,
                        Description = null,
                        Quantity = amountStr.RoundDecimal(detectedDecimalSep, 2, "."),
                        UnitOfMeasureCode = (article.Article != null && article.Article.Unit != null ? article.Article.Unit.Name : string.Empty),
                        UnitPrice = ppuStr.RoundDecimal(detectedDecimalSep, 2, ".")
                    });
                }
                else
                {
                    salesLines.Add(new SalesDocSalesHeaderSalesLine()
                    {
                        LineNo = lineNo.ToString(),
                        LineType = InvoiceXMLLineType.Description,
                        Number = string.Empty,
                        Description = article.Name.QautationFix(),
                        Quantity = string.Empty,
                        UnitOfMeasureCode = string.Empty,
                        UnitPrice = string.Empty
                    });
                }
            }

            salesHeader.SalesLine = salesLines.ToArray();

            #endregion

            #region Attachment

            var salesAttachments = new List<SalesDocSalesHeaderAttachment>();
            byte attachmentNo = 0;
            foreach (var file in order.Files)
            {
                attachmentNo++;
                salesAttachments.Add(new SalesDocSalesHeaderAttachment()
                {
                    AttachmentEntryNo = attachmentNo,
                    Filename = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = Path.GetExtension(file.FileName).Replace(".", string.Empty),
                    Attachment = CaseInvoiceOrderFileToBase64Encode(file, caseId, settings.CustomerId)
                });
            }
            salesHeader.Attachment = salesAttachments.ToArray();

            #endregion

            salesDoc.SalesHeader = salesHeader;

            return salesDoc;
        }

        private string DetectDecimalSeparator(string value)
        {
            return string.IsNullOrEmpty(value)? string.Empty : value.GetNonNumeric();
        }        

        private int GetSequenceNumber(int caseId, CaseInvoiceOrder order)
        {
            var orders = (order.CreditForOrder_Id.HasValue)?            
                        this.caseInvoiceArticleRepository.GetOrders(caseId, InvoiceOrderFetchStatus.Credits)
                        .Where(o=> o.Id < order.Id && o.CreditForOrder_Id ==  order.CreditForOrder_Id) : 
                        this.caseInvoiceArticleRepository.GetOrders(caseId, InvoiceOrderFetchStatus.Orders)
                                                         .Where(o=> o.Id < order.Id);            

            if (orders == null)
                return 1;
            else
                return orders.Count() + 1;
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

        private string OrderNoRow(decimal caseNumber, string prefix, int orderNumber, int originalOrderNumber)
        {            
            var ret = string.Empty;
            if (originalOrderNumber > 0)
                ret = string.Format("{0}{1}-{2}-{3}", prefix, caseNumber, originalOrderNumber, orderNumber);                
            else
                ret = string.Format("{0}{1}-{2}", prefix, caseNumber, orderNumber);

            return ret;
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

        private string GetSellToCustomerNo(int? departmentId)
        {
            var res = string.Empty;
            if (departmentId.HasValue)
            {
                var dep = this.departmentService.GetDepartment(departmentId.Value);
                if (dep != null)
                    res = dep.DepartmentId;
            }
            return res;
        }

        private string GetJobNo(int projectId)
        {
            var res = string.Empty;
            var project = projectService.GetProject(projectId);
            if (project != null)
            {  

                var splited = project.Name.Split(' ').ToArray();
                if (splited.Any())
                    res = splited[0];
                else
                    res = project.Name;
            }

            return res;
        }
    }
}