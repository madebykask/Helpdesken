using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    public class CaseExtraFollowersRepository : RepositoryBase<CaseExtraFollower>, ICaseExtraFollowersRepository
    {
        public CaseExtraFollowersRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<CaseExtraFollower> GetCaseExtraFollowers(int caseId)
        {
            return GetAll().Where(x => x.CaseId == caseId).ToList();
        }

        public List<CaseExtraFollower> GetCaseExtraFollowersByCaseAndFollower(int caseId, List<string> extraFollowers)
        {
            return GetAll().Where(x => x.CaseId == caseId && extraFollowers.Contains(x.Follower)).ToList();
        }

        public void SaveCaseExtraFollowers(int caseId, List<CaseExtraFollower> allFollowers)
        {
            MergeList(x => x.CaseId == caseId
                    , allFollowers
                    , (a, b) => a.Id == b.Id
                    , (a, b) =>
                    {
                        if (!a.Follower.Equals(b.Follower))
                        {
                            a.Follower = b.Follower;
                        }
                    });
            Commit();
        }
    }

    public interface ICaseExtraFollowersRepository : IRepository<CaseExtraFollower>
    {
        List<CaseExtraFollower> GetCaseExtraFollowers(int caseId);
        List<CaseExtraFollower> GetCaseExtraFollowersByCaseAndFollower(int caseId, List<string> extraFollowers);
        void SaveCaseExtraFollowers(int caseId, List<CaseExtraFollower> allFollowers);
    }
}
