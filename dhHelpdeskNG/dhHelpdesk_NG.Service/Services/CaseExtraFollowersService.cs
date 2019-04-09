using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Repositories.Cases;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Services.Services
{
    public class CaseExtraFollowersService : ICaseExtraFollowersService
    {
        private readonly ICaseExtraFollowersRepository _caseExtraFollowersRepository;
        private readonly IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower> _caseExtraFollowersToBusinessModelMapper;

        public CaseExtraFollowersService(ICaseExtraFollowersRepository caseExtraFollowersRepository,
            IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower> caseExtraFollowersToBusinessModelMapper)
        {
            _caseExtraFollowersRepository = caseExtraFollowersRepository;
            _caseExtraFollowersToBusinessModelMapper = caseExtraFollowersToBusinessModelMapper;
        }

        public List<ExtraFollower> GetCaseExtraFollowers(int caseId)
        {
            var items = _caseExtraFollowersRepository.GetCaseExtraFollowers(caseId);
            return items.Select(x => _caseExtraFollowersToBusinessModelMapper.Map(x)).ToList();
        }

        public void SaveExtraFollowers(int caseId, List<string> extraFollowers, int? userId)
        {
            var existFollowers = _caseExtraFollowersRepository.GetCaseExtraFollowersByCaseAndFollower(caseId, extraFollowers);
            var allFollowers = new List<CaseExtraFollower>();
            foreach (var extraFollower in extraFollowers)
            {
                if (EmailHelper.IsValid(extraFollower))
                {
                    var existFollower = existFollowers.SingleOrDefault(x => x.Follower.Equals(extraFollower));
                    if (existFollower != null)
                    {
                        allFollowers.Add(new CaseExtraFollower
                        {
                            Id = existFollower.Id,
                            Follower = existFollower.Follower,
                            Case_Id = existFollower.Case_Id,
                            CreatedDate = existFollower.CreatedDate,
                            CreatedByUser_Id = existFollower.CreatedByUser_Id
                        });
                    }
                    else
                    {
                        allFollowers.Add(new CaseExtraFollower
                        {
                            Follower = extraFollower,
                            Case_Id = caseId,
                            CreatedDate = DateTime.UtcNow,
                            CreatedByUser_Id = userId
                        });
                    }
                }
            }
            _caseExtraFollowersRepository.SaveCaseExtraFollowers(caseId, allFollowers);
        }

        public void DeleteByCase(int caseId)
        {
            _caseExtraFollowersRepository.Delete(x => x.Case_Id == caseId);
            _caseExtraFollowersRepository.Commit();
        }
    }

    public interface ICaseExtraFollowersService
    {
        List<ExtraFollower> GetCaseExtraFollowers(int caseId);
        void SaveExtraFollowers(int caseId, List<string> extraFollowers, int? userId);
        void DeleteByCase(int caseId);
    }
}
