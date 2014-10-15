namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Shared;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IRegionService regionService;

        private readonly IDepartmentService departmentService;

        public ProductsService(
                IUnitOfWorkFactory unitOfWorkFactory, 
                IRegionService regionService, 
                IDepartmentService departmentService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.regionService = regionService;
            this.departmentService = departmentService;
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

        public ProductsFilterData GetProductsFilterData(int customerId)
        {
            var regions = this.regionService.FindByCustomerId(customerId);
            var departments = this.departmentService.FindActiveOverviews(customerId);

            return new ProductsFilterData(regions.ToArray(), departments.ToArray());
        }

        public ProductData GetProductData(int customerId, int? productId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var productRepository = uow.GetRepository<Product>();
                var applicationRepository = uow.GetRepository<Application>();

                ProductModel product;
                if (productId.HasValue)
                {
                    product = productRepository.GetAll().MapToBusinessModel(productId.Value);
                }
                else
                {
                    product = ProductModel.CreateDefault(customerId);
                }

                var applications = applicationRepository.GetAll()
                                    .GetProductApplications(productId)
                                    .GetByCustomer(customerId)
                                    .MapToItemOverviews();

                return new ProductData(product, applications);
            }
        }

        public ProductModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var productRepository = uow.GetRepository<Product>();

                return productRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(ProductModel product)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var productRepository = uow.GetRepository<Product>();
                var applicationRepository = uow.GetRepository<Application>();
                Product entity;
                if (product.IsNew())
                {
                    entity = new Product();
                    ProductMapper.MapToEntity(product, entity);
                    entity.CreatedDate = DateTime.Now;
                    entity.ChangedDate = DateTime.Now;
                    productRepository.Add(entity);
                }
                else
                {
                    entity = productRepository.GetById(product.Id);
                    ProductMapper.MapToEntity(product, entity);
                    entity.ChangedDate = DateTime.Now;
                    productRepository.Update(entity);
                }

                entity.Applications.Clear();
                foreach (var application in product.Applications)
                {
                    var applicationEntity = applicationRepository.GetById(int.Parse(application.Value));
                    entity.Applications.Add(applicationEntity);
                }

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var productRepository = uow.GetRepository<Product>();
                var product = productRepository.GetById(id);
                product.Applications.Clear();
                productRepository.DeleteById(id);

                uow.Save();
            }
        }
    }
}