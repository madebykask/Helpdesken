using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.Invoice;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers.Api
{
    public class InvoiceApiController : BaseApiController
	{
		private readonly IInvoiceArticleService _invoiceArticleService;

		public InvoiceApiController(IInvoiceArticleService invoiceArticleService)
		{
			_invoiceArticleService = invoiceArticleService;
		}

		public List<InvoiceArticleProductAreaIndexRowModel> GetArticleProductAreaList([FromUri]InvoiceArticleProductAreaSelectedFilter filter)
		{
			var ResolveName = true;

			var res = new List<InvoiceArticleProductAreaIndexRowModel>();

			var articles = _invoiceArticleService.GetArticles(filter.CustomerId);
			if (filter.SelectedInvoiceArticles.Any())
			{
				articles = articles.Where(x => filter.SelectedInvoiceArticles.Contains(x.Id)).ToArray();
			}

			foreach (var article in articles)
			{
				var productAreas = filter.SelectedProductAreas.Any() ? article.ProductAreas.Where(x => filter.SelectedProductAreas.Contains(x.Id)).ToList() : article.ProductAreas;
				res.AddRange(productAreas.Select(productArea => new InvoiceArticleProductAreaIndexRowModel
				{
					InvoiceArticleId = article.Id,
					InvoiceArticleName = article.Name, // add desction as well
					InvoiceArticleNameEng = article.NameEng,
					InvoiceArticleNumber = article.Number,
					ProductAreaId = productArea.Id,
					ProductAreaName = productArea.ResolveFullName()
				}));

				if (!filter.SelectedProductAreas.Any() && !article.ProductAreas.Any())
				{
					res.Add(new InvoiceArticleProductAreaIndexRowModel
					{
						InvoiceArticleId = article.Id,
						InvoiceArticleName = article.Name, // add desction as well
						InvoiceArticleNameEng = article.NameEng,
						InvoiceArticleNumber = article.Number,
					});
				}
			}

			var dir = GridSortOptions.SortDirectionFromString(filter.Dir);
			if (filter.Order == 0)
			{
				if (dir == SortingDirection.Asc)
				{
					res = res.OrderBy(x => x.InvoiceArticleNumber)
							.ThenBy(x => x.InvoiceArticleName)
							.ThenBy(x => x.InvoiceArticleNameEng)
							.ToList();
				}
				else
				{
					res = res.OrderByDescending(x => x.InvoiceArticleNumber)
							.ThenByDescending(x => x.InvoiceArticleName)
							.ThenByDescending(x => x.InvoiceArticleNameEng)
							.ToList();
				}
			}
			else if (filter.Order == 1)
			{
				if (dir == SortingDirection.Desc)
				{
					res = res.OrderBy(x => x.ProductAreaName)
							.ToList();
				}
				else
				{
					res = res.OrderByDescending(x => x.ProductAreaName)
							.ToList();
				}
			}

			SessionFacade.CurrentInvoiceArticleProductAreaSearch = filter;

			return res;
		}

	}
}
