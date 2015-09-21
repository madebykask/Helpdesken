namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseSettingRepository : IRepository<CaseSettings>
    {
        string SetListCaseName(int labelId);
        void UpdateCaseSetting(CaseSettings updatedCaseSetting);
        void ReOrderCaseSetting(List<string> caseSettingIds);
    }

    public class CaseSettingRepository : RepositoryBase<CaseSettings>, ICaseSettingRepository
    {
        public CaseSettingRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public string SetListCaseName(int labelId)
        {
            var query = from cfs in this.DataContext.CaseFieldSettings
                        join cs in this.DataContext.CaseSettings on cfs.Name equals cs.Name
                        where cfs.Id == labelId
                        group cfs by new { cfs.Name } into g
                        select new CaseSettingList
                        {
                            Name = g.Key.Name
                        };

            return query.First().Name;
        }

        public void ReOrderCaseSetting(List<string> caseSettingIds)
        {
            int orderNum = 0;
            foreach (var strId in caseSettingIds)
            {
                if (!string.IsNullOrEmpty(strId))
                {
                    orderNum++;
                    var id = int.Parse(strId);
                    var caseSettingEntity = this.DataContext.CaseSettings.Find(id);
                    caseSettingEntity.ColOrder = orderNum;
                }
            }
        }

        public void UpdateCaseSetting(CaseSettings updatedCaseSetting)
        {
            var caseSettingEntity = this.DataContext.CaseSettings.Find(updatedCaseSetting.Id);

            caseSettingEntity.Name = updatedCaseSetting.Name;
            caseSettingEntity.Line = updatedCaseSetting.Line;
            caseSettingEntity.MinWidth = updatedCaseSetting.MinWidth;
            caseSettingEntity.UserGroup = updatedCaseSetting.UserGroup;
            caseSettingEntity.ColOrder = updatedCaseSetting.ColOrder;
        }
    }
}
