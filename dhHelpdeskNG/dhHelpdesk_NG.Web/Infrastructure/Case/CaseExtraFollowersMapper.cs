using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Infrastructure.Case
{
    public static class CaseExtraFollowersMapper
    {
        public static void MapToFollowerUsers(this CaseInputViewModel inputModel, ICollection<CaseExtraFollower> caseFolowerUsers)
        {
            var followers = caseFolowerUsers.Select(x => x.Follower).ToArray();
            var followerUsers = followers.Any() ? string.Join(";", followers) + ";" : string.Empty;
            inputModel.FollowerUsers = followerUsers;
        }

        public static void MapToFollowerUsers(this CaseInputViewModel inputModel, List<ExtraFollower> caseFolowerUsers)
        {
            var followers = caseFolowerUsers.Select(x => x.Follower).ToArray();
            var followerUsers = followers.Any() ? string.Join(";", followers) + ";" : string.Empty;
            inputModel.FollowerUsers = followerUsers;
        }
    }
}