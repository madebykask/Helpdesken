namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ProductsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }   

        public ProductOverview[] GetProducts(int customerId, int[] regions, int[] departments)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var productsRepository = uow.GetRepository<Product>();

                var overviews = productsRepository.GetAll()
                                .GetByCustomer(customerId)
                                .GetRegionsProducts(regions)
                                .GetDepartmentsProducts(departments)
                                .MapToOverviews();

                return overviews;
            }
        }
    }
}