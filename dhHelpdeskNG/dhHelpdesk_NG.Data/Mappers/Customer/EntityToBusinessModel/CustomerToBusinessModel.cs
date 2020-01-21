namespace DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.Domain;

    public sealed class CustomerToBusinessModel : IEntityToBusinessModelMapper<Customer, CustomerOverview>
    {
        public CustomerOverview Map(Customer entity)
        {
            return new CustomerOverview
                       {
                           Id = entity.Id,
						   Active = entity.Status == 1
                       };
        }
    }
}