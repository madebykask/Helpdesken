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

    public sealed class IkeaExcelImporter : IInvoiceImporter
    {
        private readonly IProductAreaService productAreaService;

        private readonly IInvoiceArticleService invoiceArticleService;

        public IkeaExcelImporter(
                IProductAreaService productAreaService, 
                IInvoiceArticleService invoiceArticleService)
        {
            this.productAreaService = productAreaService;
            this.invoiceArticleService = invoiceArticleService;
        }

        public ArticlesImportData ImportArticles(Stream stream)
        {
            using (var excelPackage = new ExcelPackage(stream))
            {
                var worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                {
                    return null;
                }

                var rowsCount = worksheet.Dimension.End.Row;
                if (rowsCount < 2)
                {
                    return null;
                }

                var productAreas = new List<ProductAreaOverview>();
                var articles = new List<InvoiceArticle>();
                var units = new List<InvoiceArticleUnit>();

                for (var i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    var productAreaParent = worksheet.Cells[i, 3].Value.ToTrimString();
                    var productAreaChild = worksheet.Cells[i, 4].Value.ToTrimString();
                    var articleNumber = worksheet.Cells[i, 5].Value.ToTrimString();
                    var packageName = worksheet.Cells[i, 6].Value.ToTrimString();
                    var articleName = worksheet.Cells[i, 7].Value.ToTrimString();
                    var articleNameEng = worksheet.Cells[i, 8].Value.ToTrimString();
                    var articleDescription = worksheet.Cells[i, 9].Value.ToTrimString();
                    var unitName = worksheet.Cells[i, 11].Value.ToTrimString();
                    var ppuString = worksheet.Cells[i, 12].Value.ToTrimString();

                    if (articleName == null)
                    {
                        articleName = string.Empty;
                    }

                    if (articleNameEng == null)
                    {
                        articleNameEng = string.Empty;
                    }

                    if (string.IsNullOrEmpty(productAreaParent) || 
                        string.IsNullOrEmpty(productAreaChild))
                    {
                        continue;
                    }

                    int an;
                    if (!int.TryParse(articleNumber, out an))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(articleName) && 
                        string.IsNullOrEmpty(articleNameEng))
                    {
                        continue;
                    }

                    var parentArea = productAreas.FirstOrDefault(a => a.Name.EqualWith(productAreaParent));
                    if (parentArea == null)
                    {
                        parentArea = new ProductAreaOverview { Name = productAreaParent };
                        productAreas.Add(parentArea);
                    }

                    var childArea = productAreas.FirstOrDefault(a => a.Name.EqualWith(productAreaChild));
                    if (childArea == null)
                    {
                        childArea = new ProductAreaOverview { Name = productAreaChild };
                        productAreas.Add(childArea);
                        parentArea.Children.Add(childArea);
                    }

                    var unit = units.FirstOrDefault(u => u.Name.EqualWith(unitName));
                    if (unit == null &&
                        !string.IsNullOrEmpty(unitName))
                    {
                        unit = new InvoiceArticleUnit(unitName);
                        units.Add(unit);
                    }

                    var package = articles.FirstOrDefault(a => a.Name.Equals(packageName));
                    if (package == null &&
                        !string.IsNullOrEmpty(packageName))
                    {
                        package = new InvoiceArticle(
                                    an,
                                    packageName,
                                    childArea);
                        articles.Add(package);
                    }

                    var article = articles.FirstOrDefault(a => a.Name.EqualWith(articleName) && a.NameEng.EqualWith(articleNameEng));
                    if (article == null)
                    {
                        decimal? ppu = null;
                        decimal ppuValue;
                        if (decimal.TryParse(ppuString, out ppuValue))
                        {
                            ppu = ppuValue;
                        }

                        article = new InvoiceArticle(
                                    package,
                                    an,
                                    articleName,
                                    articleNameEng,
                                    articleDescription,
                                    childArea,
                                    unit,
                                    ppu);
                        articles.Add(article);
                    }
                }

                return new ArticlesImportData(
                                    productAreas.ToArray(),
                                    articles.ToArray(),
                                    units.ToArray());
            }
        }

        public void SaveImportedArticles(ArticlesImportData data, int customerId)
        {
            if (data == null)
            {
                return;
            }

            var units = this.invoiceArticleService.GetUnits(customerId);
            foreach (var unit in data.Units)
            {
                unit.CustomerId = customerId;
                var savedUnit = units.FirstOrDefault(u => u.Name.EqualWith(unit.Name));
                if (savedUnit != null)
                {
                    unit.Id = savedUnit.Id;
                }
                else
                {
                    unit.Id = this.invoiceArticleService.SaveUnit(unit);
                }

                var articlesWithUnit = data.Articles.Where(a => a.Unit != null && a.Unit.Name.EqualWith(unit.Name));
                foreach (var articleWithUnit in articlesWithUnit)
                {
                    articleWithUnit.UnitId = unit.Id;
                }
            }

            var productAreas = this.productAreaService.GetProductAreaOverviews(customerId);
            foreach (var productArea in data.ProductAreas)
            {
                productArea.CustomerId = customerId;
                var savedProductArea = productAreas.FirstOrDefault(a => a.Name.EqualWith(productArea.Name));
                if (savedProductArea != null)
                {
                    productArea.Id = savedProductArea.Id;
                }
                else
                {
                    var parentProductArea = data.ProductAreas.FirstOrDefault(a => a.Children.Any(c => c.Name.EqualWith(productArea.Name)));
                    if (parentProductArea != null)
                    {
                        parentProductArea.CustomerId = customerId;
                        var savedParentProductArea = productAreas.FirstOrDefault(a => a.Name.EqualWith(parentProductArea.Name));
                        if (savedParentProductArea != null)
                        {
                            parentProductArea.Id = savedParentProductArea.Id;
                        }
                        else
                        {
                            parentProductArea.Id = this.productAreaService.SaveProductArea(parentProductArea);
                        }

                        productArea.ParentId = parentProductArea.Id;
                    }

                    productArea.Id = this.productAreaService.SaveProductArea(productArea);
                }

                var articlesWithProductArea = data.Articles.Where(a => a.ProductArea != null && a.ProductArea.Name.EqualWith(productArea.Name));
                foreach (var articleWithProductArea in articlesWithProductArea)
                {
                    articleWithProductArea.ProductAreaId = productArea.Id;
                }
            }

            foreach (var article in data.Articles)
            {
                if (article.Id > 0)
                {
                    continue;
                }

                article.CustomerId = customerId;
                if (article.Parent != null)
                {
                    article.Parent.Id = this.invoiceArticleService.SaveArticle(article.Parent);
                    article.ParentId = article.Parent.Id;
                }

                article.Id = this.invoiceArticleService.SaveArticle(article);
            }
        }
    }
}