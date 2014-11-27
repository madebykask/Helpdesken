namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Computers;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class ComputersService : IComputersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ComputersService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public ComputerOverview[] GetComputerOverviews(int customerId, int productId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var computersRep = uow.GetRepository<Computer>();
                var softwareRep = uow.GetRepository<Software>();
                var applicationsRep = uow.GetRepository<Application>();

                return computersRep.GetAll()
                        .GetByProduct(softwareRep.GetAll(), applicationsRep.GetAll(), customerId, productId)
                        .MapToOverviews();
            }
        }
    }
}