using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using DH.Helpdesk.WebApi.Models.Output;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/CaseFavorites")]
    public class CaseFavoritesController: BaseApiController
    {
        private readonly ICaseService _caseService;
        private readonly ICustomerUserService _customerUserService;

        public CaseFavoritesController(ICaseService caseService,
            ICustomerUserService customerUserService)
        {
            _caseService = caseService;
            _customerUserService = customerUserService;
        }

        [HttpGet]
        [Route("customer")]
        public async Task<List<CaseFavoriteFilterModel>> Customer(int cid)
        {
            var favorites = await _caseService.GetMyFavoritesWithFieldsAsync(cid, UserId);

            //#71782: skip filters with ClosingReason and Closing date since mobile app supports only cases in progress only
            favorites = 
                favorites.Where(f => f.Fields != null && 
                                     (f.Fields.ClosingReasonFilter == null || !f.Fields.ClosingReasonFilter.Any()) && 
                                     (f.Fields.ClosingDateFilter == null || !f.Fields.ClosingDateFilter.HasValues)).ToList();

            var model = favorites.Any()
                ? favorites.Select(f => new CaseFavoriteFilterModel()
                {
                    Id = f.Id,
                    Name = f.Name,
                    Fields = f.Fields.ToDictionary().Select(kv => new ItemOverview(kv.Key, kv.Value)).ToList()
                }).OrderBy(f => f.Name).ToList()
                : null;

            return model;
        }

        [HttpGet]
        [SkipCustomerAuthorization]
        [Route("user")]
        public async Task<List<CustomerCaseFavoriteFilterModel>> ForCurrentUser()
        {
            var model = new List<CustomerCaseFavoriteFilterModel>();
            var customers = _customerUserService.GetCustomerUsersForHomeIndexPage(UserId);
            if (!customers.Any())
                return model;

            foreach (var customer in customers)
            {
                var customerFilter = new CustomerCaseFavoriteFilterModel();
                customerFilter.CustomerId = customer.Customer.Customer_Id;
                customerFilter.CustomerName = customer.Customer.Customer.Name;
                
                var favorites = await _caseService.GetMyFavoritesWithFieldsAsync(customer.Customer.Customer_Id, UserId);

                //#71782: skip filters with ClosingReason and Closing date since mobile app supports only cases in progress only
                favorites =
                    favorites.Where(f => f.Fields != null &&
                                         (f.Fields.ClosingReasonFilter == null ||
                                          !f.Fields.ClosingReasonFilter.Any()) &&
                                         (f.Fields.ClosingDateFilter == null || !f.Fields.ClosingDateFilter.HasValues))
                        .ToList();

                customerFilter.Favorites = favorites.Any()
                    ? favorites.Select(f => new CaseFavoriteFilterModel()
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Fields = f.Fields.ToDictionary().Select(kv => new ItemOverview(kv.Key, kv.Value)).ToList()
                    }).OrderBy(f => f.Name).ToList()
                    : null;
                model.Add(customerFilter);
            }

            return model.OrderBy(c => c.CustomerName).ToList();
        }

    }
}