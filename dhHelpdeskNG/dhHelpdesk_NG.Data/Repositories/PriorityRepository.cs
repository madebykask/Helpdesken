using System.Data.Entity;
using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region PRIORITY

    public interface IPriorityRepository : IRepository<Priority>
    {
        void ReOrderPriorities(List<string> priorityIds);
        void ResetDefault(int exclude, int customerId);
        void ResetEmailDefault(int exclude, int customerId);
    }

    public class PriorityRepository : RepositoryBase<Priority>, IPriorityRepository
    {
        public PriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


        public void ReOrderPriorities(List<string> priorityIds)
        {
            int orderNum = 0;
            foreach (var strId in priorityIds)
            {
                if (!string.IsNullOrEmpty(strId))
                {
                    orderNum++;
                    var id = int.Parse(strId);
                    var priorityEntity = this.DataContext.Priorities.Find(id);
                    priorityEntity.OrderNum = orderNum;
                }
            }
        }

        public void ResetDefault(int exclude, int customerId)
        {
            foreach (Priority obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

        public void ResetEmailDefault(int exclude, int customerId)
        {
            foreach (Priority obj in this.GetMany(s => s.IsEmailDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
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
        string GetPriorityInfoTextByImpactAndUrgency(int impactId, int urgentId, int languageId);
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

        public string GetPriorityInfoTextByImpactAndUrgency(int impactId, int urgentId, int languageId)
        {
            var priorityUrgent = DataContext.PriorityImpactUrgencies
                .Include(x => x.Priority.PriorityLanguages)
                .Where(x => x.Priority.InformUser == 1 && !string.IsNullOrEmpty(x.Priority.InformUserText))
                .FirstOrDefault(x => x.Impact_Id == impactId && x.Urgency_Id == urgentId);
            if (priorityUrgent?.Priority != null)
            {
                var infoText = priorityUrgent.Priority.InformUserText;
                var translate =
                    priorityUrgent.Priority.PriorityLanguages.FirstOrDefault(x => x.Language_Id == languageId);
                if (!string.IsNullOrEmpty(translate?.InformUserText))
                {
                    infoText = translate.InformUserText;
                }
                return infoText;
            }
            return string.Empty;
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
