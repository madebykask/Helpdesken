namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Shared;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ApplicationsService : IApplicationsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ApplicationsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ApplicationOverview[] GetApplications(int customerId, string name, bool onlyConnected)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();

                var overviews = applicationRepository.GetAll()
                                .GetOnlyConnectedCustomerApplications(customerId, onlyConnected)
                                .GetByName(name)
                                .MapToOverviews();

                return overviews;
            }
        }

        public ApplicationData GetApplicationData(int customerId, int? applicationId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
                var productRepository = uow.GetRepository<Product>();
 
                ApplicationModel application;
                if (applicationId.HasValue)
                {
                    application = applicationRepository.GetAll().MapToBusinessModel(applicationId.Value);
                }
                else
                {
                    application = ApplicationModel.CreateDefault(customerId);
                }

                var products = productRepository.GetAll()
                                .GetByCustomer(customerId)
                                .MapToItemOverviews();

                return new ApplicationData(application, products);                
            }
        }

        public ApplicationModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();

                return applicationRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(ApplicationModel application)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
                var productRepository = uow.GetRepository<Product>();

                Application entity;
                if (application.IsNew())
                {
                    entity = new Application();
                    ApplicationMapper.MapToEntity(application, entity);
                    entity.CreatedDate = DateTime.Now;
                    entity.ChangedDate = DateTime.Now;
                    applicationRepository.Add(entity);
                }
                else
                {
                    entity = applicationRepository.GetById(application.Id);
                    ApplicationMapper.MapToEntity(application, entity);
                    entity.ChangedDate = DateTime.Now;
                    applicationRepository.Update(entity);
                }

                entity.Products.Clear();
                if (application.ProductId.HasValue)
                {
                    var product = productRepository.GetById(application.ProductId.Value);
                    entity.Products.Add(product);
                }

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
                var application = applicationRepository.GetById(id);
                application.Products.Clear();
                applicationRepository.DeleteById(id);

                uow.Save();
            }
        }
    }
}