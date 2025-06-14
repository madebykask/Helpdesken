﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkModelFactory.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LinkModelFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Link.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Link.Output;
    using DH.Helpdesk.Web.Models.Link;

    /// <summary>
    /// The link model factory.
    /// </summary>
    public sealed class LinkModelFactory : ILinkModelFactory
    {
        /// <summary>
        /// The get links view model.
        /// </summary>
        /// <param name="linkOverviews">
        /// The link overviews.
        /// </param>
        /// <returns>
        /// The <see cref="LinksInfoViewModel"/>.
        /// </returns>
        public LinksInfoViewModel GetLinksViewModel(IEnumerable<LinkOverview> linkOverviews)
        {
            var model = new LinksInfoViewModel();

            var customerGroups = linkOverviews.GroupBy(l => l.CustomerId);
            foreach (var customerGroup in customerGroups)
            {
                var customer = new LinkCustomerGroupViewModel();
                var c = customerGroup.First();
                customer.CustomerId = c.CustomerId;
                customer.CustomerName = c.CustomerName;

                var categories = linkOverviews
                                    .Where(l => l.CustomerId == customer.CustomerId)
                                    .Select(l => new { LinkGroupName = l.LinkGroupName, LinkGroupId = l.LinkGroupId })
                                    .Distinct()
                                    .OrderBy(l => l.LinkGroupName);

                foreach (var cat in categories)
                {
                    var category = new LinkCategoryGroupViewModel();
                    category.CategoryName = cat.LinkGroupName;
                    category.CategoryId = cat.LinkGroupId;
                    category.Links.AddRange(linkOverviews
                                            .Where(l => l.CustomerId == customer.CustomerId && l.LinkGroupName == cat.LinkGroupName));
                    customer.Categories.Add(category);
                }

                model.CustomerGroups.Add(customer);
            }

            return model;
        }         
    }
}