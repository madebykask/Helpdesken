namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Statistics.Output;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Customers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;

    public sealed class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public StatisticsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public StatisticsOverview GetStatistics(
                        int[] customers, 
                        int userId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseRepository = uow.GetRepository<Case>();
                var problemsRep = uow.GetRepository<Problem>();

                var statistics = caseRepository.GetAll()
                                .GetCustomersCases(customers)
                                .MapToStatistics(problemsRep.GetAll(), userId);

                return statistics;
            }
        }
    }
}