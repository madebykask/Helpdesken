// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AjaxController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the AjaxController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Interfaces;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    /// <summary>
    /// The ajax controller.
    /// </summary>
    public class AjaxController : Controller
    {
        /// <summary>
        /// The product area service.
        /// </summary>
        private readonly IProductAreaService productAreaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AjaxController"/> class.
        /// </summary>
        /// <param name="productAreaService">
        /// The product area service.
        /// </param>
        public AjaxController(IProductAreaService productAreaService)
        {
            this.productAreaService = productAreaService;
        }

        /// <summary>
        /// The product area summary.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [HttpGet]
        public JsonResult ProductArea(int customerId)
        {
            var list = new HierarchyList();
            var all = this.productAreaService.GetProductAreaOverviews(customerId);
            this.FillHierarchyList(null, list, all);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The fill hierarchy list.
        /// </summary>
        /// <param name="level">
        /// The item.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="all">
        /// The all.
        /// </param>
        private void FillHierarchyList(
            IEnumerable<IHierarchyItem> level,
            HierarchyList list, 
            IEnumerable<IHierarchyItem> all)
        {
            var brothers = level != null ? 
                all.Where(i => level.Select(l => (int?)l.Id).ToArray().Contains(i.ParentId)) :
                all.Where(i => !i.ParentId.HasValue);
            if (!brothers.Any())
            {
                return;
            }

            var group = new ListGroup();
            var itemsList = new List<ListItem>();
            itemsList.Add(new ListItem()
            {
                Name = string.Empty
            });
            foreach (var brother in brothers)
            {
                itemsList.Add(new ListItem()
                                  {
                                      Id = brother.Id,
                                      ParentId = brother.ParentId,
                                      Name = brother.Name,
                                      Description = brother.Description
                                  });
            }
            group.Items = itemsList.ToArray();
            var groupsList = new List<ListGroup>();
            if (list.Groups != null)
            {
                groupsList.AddRange(list.Groups);
            }
            groupsList.Add(group);
            list.Groups = groupsList.ToArray();
            this.FillHierarchyList(brothers, list, all);
        }
    }
}
