using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Areas.Admin.Infrastructure.Mappers
{
    public static class CustomerUserEntityToModelMapper
    {
        public static CustomerUserForEdit MapToCustomerUserEdit(this CustomerUser entity)
        {
            return new CustomerUserForEdit()
            {
                UserId = entity.User_Id,
                CustomerId = entity.Customer_Id,
                CustomerName = entity.Customer.Name,
                UserInfoPermission = entity.UserInfoPermission.ToBool(),
                CaptionPermission = entity.CaptionPermission.ToBool(),
                ContactBeforeActionPermission = entity.ContactBeforeActionPermission.ToBool(),
                PriorityPermission = entity.PriorityPermission.ToBool(),
                StateSecondaryPermission = entity.StateSecondaryPermission.ToBool(),
                WatchDatePermission = entity.WatchDatePermission.ToBool(),
                RestrictedCasePermission = entity.RestrictedCasePermission
            };
        }
    }
}