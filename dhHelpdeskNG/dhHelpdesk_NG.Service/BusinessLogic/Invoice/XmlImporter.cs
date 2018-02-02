using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Services.Services;

    using OfficeOpenXml;

    public sealed class XmlImporter : IInvoiceImporter
    {
        private readonly IProductAreaService productAreaService;

        private readonly IInvoiceArticleService invoiceArticleService;

        public XmlImporter(
                IProductAreaService productAreaService, 
                IInvoiceArticleService invoiceArticleService)
        {
            this.productAreaService = productAreaService;
            this.invoiceArticleService = invoiceArticleService;
        }

        public ArticlesImportData ImportArticles(Stream stream, DateTime lastSyncDate, string filter)
        {
            var units = new List<InvoiceArticleUnit>();
            var articles = new List<InvoiceArticle>();
            var result = new ArticlesImportData(articles, units);
            try
            {
                var doc = XDocument.Load(stream);
                if (doc.Root != null)
                {
                    var xmlArticles = (from items in doc.Root.Descendants("Item")
                        let number = items.Element("No")
                        let name = items.Element("Description")
                        let nameEng = items.Element("Description2")
                        let description = items.Element("Description3")
                        let unitOfMeasure = items.Element("UnitOfMeasure")
                        let unitPrice = items.Element("UnitPrice")
                        let blocked = items.Element("Blocked")
                        let showInCHS = items.Element("ShowInCHS")
                        let textDemand = items.Element("TextDemand")
                        let itemCategoryCode = items.Element("ItemCategoryCode")
                        select new
                        {
                            Number = number != null ? number.Value : string.Empty,
                            Name = name != null ? name.Value : string.Empty,
                            NameEng = nameEng != null ? nameEng.Value : string.Empty,
                            Description = description != null ? description.Value : string.Empty,
                            UnitName = unitOfMeasure != null ? unitOfMeasure.Value : string.Empty,
                            Ppu = unitPrice != null ? unitPrice.Value : string.Empty,
                            Blocked = blocked != null && XmlConvert.ToBoolean(blocked.Value),
                            ShowInCHS = showInCHS != null && XmlConvert.ToBoolean(showInCHS.Value),
                            TextDemand = textDemand != null && XmlConvert.ToBoolean(textDemand.Value),
                            ItemCategoryCode = itemCategoryCode != null ? itemCategoryCode.Value : string.Empty,
                        }).ToList();

                    foreach (var xmlArticle in xmlArticles)
                    {
                        if (string.IsNullOrEmpty(xmlArticle.Name) || string.IsNullOrWhiteSpace(xmlArticle.Name) ||
                            string.IsNullOrEmpty(xmlArticle.NameEng) || string.IsNullOrWhiteSpace(xmlArticle.NameEng))
                            result.Errors.Add("Ogiltigt artikelnamn");
                        if (string.IsNullOrEmpty(xmlArticle.Number) || string.IsNullOrWhiteSpace(xmlArticle.Number))
                            result.Errors.Add("Ogiltigt artikelnummer");

                        if (result.Errors.Any())
                            return result;

                        if (string.IsNullOrEmpty(filter) || (!string.IsNullOrEmpty(filter) && xmlArticle.ItemCategoryCode.ToLower().Equals(filter.ToLower())))
                        {
                            var unit = units.FirstOrDefault(u => u.Name.EqualWith(xmlArticle.UnitName));
                            if (unit == null && !string.IsNullOrEmpty(xmlArticle.UnitName))
                            {
                                unit = new InvoiceArticleUnit(xmlArticle.UnitName);
                                units.Add(unit);
                            }

                            var article = articles.FirstOrDefault(a => a.Name.EqualWith(xmlArticle.Name) && a.NameEng.EqualWith(xmlArticle.NameEng));
                            if (article == null)
                            {
                                decimal? ppu = null;
                                decimal ppuValue;
                                if (decimal.TryParse(xmlArticle.Ppu, out ppuValue))
                                {
                                    ppu = ppuValue;
                                }

                                var blocked = xmlArticle.Blocked || !xmlArticle.ShowInCHS;

                                article = new InvoiceArticle(
                                    xmlArticle.Number,
                                    xmlArticle.Name,
                                    xmlArticle.NameEng,
                                    xmlArticle.Description,
                                    unit,
                                    ppu,
                                    xmlArticle.TextDemand,
                                    blocked,
                                    lastSyncDate);
                                articles.Add(article);
                            }
                        }
                    }
                }

                return result;
            }
            catch (XmlException e)
            {
                result.Errors.Add("Ogiltig XML. Line, ställning");
                result.Errors.Add(string.Format(": ({0}, {1})", e.LineNumber, e.LinePosition));
                return result;
            }
        }

        public void SaveImportedArticles(ArticlesImportData data, int customerId, DateTime lastSyncDate)
        {
            if (data == null)
            {
                return;
            }

            var units = invoiceArticleService.GetUnits(customerId);
            foreach (var unit in data.Units)
            {
                unit.CustomerId = customerId;
                var savedUnit = units.FirstOrDefault(u => u.Name.EqualWith(unit.Name));
                unit.Id = savedUnit != null ? savedUnit.Id : invoiceArticleService.SaveUnit(unit);

                var articlesWithUnit = data.Articles.Where(a => a.Unit != null && a.Unit.Name.EqualWith(unit.Name));
                foreach (var articleWithUnit in articlesWithUnit)
                {
                    articleWithUnit.UnitId = unit.Id;
                }
            }

            var articles = this.invoiceArticleService.GetArticles(customerId);
            foreach (var article in data.Articles)
            {
                article.CustomerId = customerId;
                var savedArticle = articles.FirstOrDefault(a => a.Number == article.Number //&&
                                                   // a.Name.EqualWith(article.Name) &&
                                                   // a.NameEng.EqualWith(article.NameEng)
                                                   );
                if (savedArticle != null)
                {
                    article.Id = savedArticle.Id;
                    article.IsActive = 1;
                }
                else
                {
                    article.IsActive = 1;
                }
            }
            invoiceArticleService.DeactivateArticlesBySyncDate(customerId, lastSyncDate);
            invoiceArticleService.SaveArticles(data.Articles);
        }
    }
}