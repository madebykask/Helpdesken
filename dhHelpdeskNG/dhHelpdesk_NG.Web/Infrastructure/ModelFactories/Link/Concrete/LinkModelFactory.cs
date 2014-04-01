using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Link.Output;
using DH.Helpdesk.Web.Models.Link;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Link.Concrete
{
    public sealed class LinkModelFactory : ILinkModelFactory
    {
        public LinksInfoViewModel GetLinksViewModel(IEnumerable<LinkOverview> linkOverviews)
        {
            var model = new LinksInfoViewModel();

            var customerGroups = linkOverviews.GroupBy(l => l.Customer_Id);
            foreach (var customerGroup in customerGroups)
            {
                var customer = new LinkCustomerGroupViewModel();
                var c = customerGroup.First();
                customer.CustomerId = c.Customer_Id;
                customer.CustomerName = c.CustomerName;

                var categoryNames = linkOverviews
                                    .Where(l => l.Customer_Id == customer.CustomerId)
                                    .Select(l => l.LinkGroupName)
                                    .Distinct();
                foreach (var categoryName in categoryNames)
                {
                    var category = new LinkCategoryGroupViewModel();
                    category.CategoryName = categoryName;
                    category.Links.AddRange(linkOverviews
                                            .Where(l => l.Customer_Id == customer.CustomerId && l.LinkGroupName == categoryName)
                                            .OrderBy(l => l.URLName));
                    customer.Categories.Add(category);
                }

                model.CustomerGroups.Add(customer);
            }

            return model;
        }         
    }
}