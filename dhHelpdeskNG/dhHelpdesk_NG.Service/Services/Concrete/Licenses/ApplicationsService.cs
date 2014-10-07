namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ApplicationsService : IApplicationsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ApplicationsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ApplicationOverview[] GetApplications(int customerId, bool onlyConnected)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
                var productRepository = uow.GetRepository<Product>();

                var overviews = applicationRepository.GetAll()
                                .GetOnlyConnectedCustomerApplications(customerId, onlyConnected)
                                .MapToOverviews(productRepository.GetAll());

                return overviews;
            }
        }

        public ApplicationData GetApplicationData(int customerId, int? applicationId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var applicationRepository = uow.GetRepository<Application>();
 
                ApplicationModel application;
                if (applicationId.HasValue)
                {
                    application = applicationRepository.GetAll().MapToBusinessModel(applicationId.Value);
                }
                else
                {
                    application = ApplicationModel.CreateDefault(customerId);
                }

                return new ApplicationData(application);                
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

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Application>();

                repository.DeleteById(id);

                uow.Save();
            }
        }
    }
}