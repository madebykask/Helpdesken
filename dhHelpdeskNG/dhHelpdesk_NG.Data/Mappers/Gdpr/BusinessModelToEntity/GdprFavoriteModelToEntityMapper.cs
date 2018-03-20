using System.Linq;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Mappers.Gdpr.BusinessModelToEntity
{
    public class GdprFavoriteModelToEntityMapper : IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite>
    {
        public void Map(GdprFavoriteModel model, GDPRDataPrivacyFavorite entity)
        {
            entity.Id = model.Id;
            entity.Name = model.Name;
            
            //favorites fields
            entity.CustomerId = model.CustomerId;
            entity.RetentionPeriod = model.RetentionPeriod;
            entity.CalculateRegistrationDate = model.CalculateRegistrationDate;
            entity.RegisterDateFrom = model.RegisterDateFrom;
            entity.RegisterDateTo = model.RegisterDateTo;
            entity.ClosedOnly = model.ClosedOnly;

            entity.FieldsNames =
                model.FieldsNames != null && model.FieldsNames.Any() ? string.Join(",", model.FieldsNames) : "";

            entity.ReplaceDataWith = model.ReplaceDataWith;
            entity.ReplaceDatesWith = model.ReplaceDatesWith;
            entity.RemoveCaseAttachments = model.RemoveCaseAttachments;
            entity.RemoveLogAttachments = model.RemoveLogAttachments;
        }
    }
}