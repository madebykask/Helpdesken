namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region PRIORITY

    public interface IPriorityRepository : IRepository<Priority>
    {
        void ResetDefault(int exclude);
        void ResetEmailDefault(int exclude);
    }

    public class PriorityRepository : RepositoryBase<Priority>, IPriorityRepository
    {
        public PriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude)
        {
            foreach (Priority obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

        public void ResetEmailDefault(int exclude)
        {
            foreach (Priority obj in this.GetMany(s => s.IsEmailDefault == 1 && s.Id != exclude))
            {
                obj.IsEmailDefault = 0;
                this.Update(obj);
            }
        }
    }

    #endregion

    #region PRIORITYIMPACTURGENCY

    public interface IPriorityImpactUrgencyRepository : IRepository<PriorityImpactUrgency>
    {
        int GetPriorityIdByImpactAndUrgency(int impactId, int urgencyId);
    }

    public class PriorityImpactUrgencyRepository : RepositoryBase<PriorityImpactUrgency>, IPriorityImpactUrgencyRepository
    {
        public PriorityImpactUrgencyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public int GetPriorityIdByImpactAndUrgency(int impactId, int urgencyId)
        {
            var p = this.DataContext.PriorityImpactUrgencies.Where(x => x.Impact_Id == impactId && x.Urgency_Id == urgencyId).FirstOrDefault();
            return p != null ? p.Priority_Id : 0;
        }
    }

    #endregion

    #region PRIORITYLANGUAGE

    public interface IPriorityLanguageRepository : IRepository<PriorityLanguage>
    {
    }

    public class PriorityLanguageRepository : RepositoryBase<PriorityLanguage>, IPriorityLanguageRepository
    {
        public PriorityLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
