namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ProductsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ProductOverview[] GetProducts(int customerId, int? regionId, int? departmentId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var productsRepository = uow.GetRepository<Product>();
                var licensesRepository = uow.GetRepository<License>();

                var products = productsRepository.GetAll();
                var licenses = licensesRepository.GetAll();
                
                var query = from p in products
                            where p.Customer_Id == customerId
                            select new ProductOverview
                                        {
                                            ProductId = p.Id,
                                            ProductName = p.Name
                                        };

                return query.ToArray();
            }
        }
    }
}