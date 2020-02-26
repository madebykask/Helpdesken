namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
	using DH.Helpdesk.BusinessData.Models.Orders.Order;
	using DH.Helpdesk.Dal.Infrastructure.Context;
	using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;
	using System.Collections.Generic;

	public interface INewOrderModelFactory
    {
        FullOrderEditModel Create(
                        string temporatyId, 
                        NewOrderEditData data,
                        IWorkContext workContext,
                        int? orderTypeId,
						List<string> fileUploadWhiteList);
    }
}