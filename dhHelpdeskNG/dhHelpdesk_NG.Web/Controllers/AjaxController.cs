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

    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
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
        /// <param name="productArea">
        /// The product area.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [HttpGet]
        public JsonResult ProductArea(int customerId, int? productArea)
        {
            var list = new HierarchyList();
            var groups = new List<ListGroup>();

            int? parentId = productArea;
            do
            {
                var levelItems = this.productAreaService.GetSameLevelOverviews(customerId, parentId);
                if (levelItems != null && levelItems.Any())
                {
                    var group = new ListGroup();
                    var items = new List<ListItem>();
                    foreach (var item in levelItems)
                    {
                        items.Add(
                            new ListItem()
                                {
                                    Id = item.Id,
                                    ParentId = item.ParentId,
                                    Name = item.Name,
                                    Description = item.Description
                                });
                    }
                    group.Items = items.ToArray();
                    groups.Add(group);
                    parentId = levelItems.First().ParentId;
                }
            }
            while (parentId.HasValue);

            list.Groups = groups.ToArray();
            list.Groups.Reverse();

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The product area children.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="productArea">
        /// The product area.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [HttpGet]
        public JsonResult ProductAreaChildren(int customerId, int productArea)
        {
            return new JsonResult();
        }
    }
}
