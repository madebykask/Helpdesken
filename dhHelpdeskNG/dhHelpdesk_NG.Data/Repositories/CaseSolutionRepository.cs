namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region CASESOLUTION

    public interface ICaseSolutionRepository : IRepository<CaseSolution>
    {
    }

    public class CaseSolutionRepository : RepositoryBase<CaseSolution>, ICaseSolutionRepository
    {
        public CaseSolutionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region CASESOLUTIONCATEGORY

    public interface ICaseSolutionCategoryRepository : IRepository<CaseSolutionCategory>
    {
        void ResetDefault(int exclude);
    }

    public class CaseSolutionCategoryRepository : RepositoryBase<CaseSolutionCategory>, ICaseSolutionCategoryRepository
    {
        public CaseSolutionCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (CaseSolutionCategory obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }
    }
       
    #endregion

    #region CASESOLUTIONSCHEDULE

    public interface ICaseSolutionScheduleRepository : IRepository<CaseSolutionSchedule>
    {
    }

    public class CaseSolutionScheduleRepository : RepositoryBase<CaseSolutionSchedule>, ICaseSolutionScheduleRepository
    {
        public CaseSolutionScheduleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
