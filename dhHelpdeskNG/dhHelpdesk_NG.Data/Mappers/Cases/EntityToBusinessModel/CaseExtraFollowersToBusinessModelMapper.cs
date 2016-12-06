using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    public class CaseExtraFollowersToBusinessModelMapper : IEntityToBusinessModelMapper<CaseExtraFollower, ExtraFollower>
    {
        public ExtraFollower Map(CaseExtraFollower entity)
        {
            return new ExtraFollower
            {
                Id = entity.Id,
                Follower = entity.Follower,
                CaseId = entity.Case_Id,
                CreatedByUserId = entity.CreatedByUser_Id,
                CreatedDate = entity.CreatedDate
            };
        }
    }
}
