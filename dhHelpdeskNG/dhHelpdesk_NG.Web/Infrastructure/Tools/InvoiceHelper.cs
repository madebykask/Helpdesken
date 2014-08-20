namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Collections.Generic;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public static class InvoiceHelper
    {
        public static CaseInvoiceArticle[] ToCaseInvoiceArticles(string articles)
        {
            var serializer = new JavaScriptSerializer();
            var res = serializer.Deserialize<CaseAtricle[]>(articles);
            var list = new List<CaseInvoiceArticle>();
            if (res != null)
            {
                foreach (var item in res)
                {
                    list.Add(new CaseInvoiceArticle(
                                item.Id,
                                item.CaseId,
                                null,
                                item.ArticleId,
                                null,
                                item.Name,
                                item.Amount,
                                item.Ppu,
                                item.Position,
                                item.IsInvoiced));
                }
            }
            return list.ToArray();
        }

        private class CaseAtricle
        {
            public CaseAtricle()
            {
                
            }

            public int Id { get; set; }

            public int CaseId { get; set; }

            public int? ArticleId { get; set; }

            public string Name { get; set; }

            public int? Amount { get; set; }

            public decimal? Ppu { get; set; }

            public short Position { get; set; }

            public bool IsInvoiced { get; set; }
        }
    }
}