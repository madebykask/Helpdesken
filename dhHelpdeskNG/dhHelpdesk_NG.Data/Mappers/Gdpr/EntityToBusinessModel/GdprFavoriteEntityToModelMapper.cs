using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Mappers.Gdpr.EntityToBusinessModel
{
    public class GdprFavoriteEntityToModelMapper: IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel>
    {
        public GdprFavoriteModel Map(GDPRDataPrivacyFavorite entity)
        {
            var model = new GdprFavoriteModel
            {
                Id = entity.Id,
                Name = entity.Name,
                CustomerId = entity.CustomerId,
                RetentionPeriod = entity.RetentionPeriod,
                CalculateRegistrationDate = entity.CalculateRegistrationDate,
                RegisterDateFrom = entity.RegisterDateFrom,
                RegisterDateTo = entity.RegisterDateTo,
                ClosedOnly = entity.ClosedOnly,
                FieldsNames = ConvertToList(entity.FieldsNames),
                ReplaceDataWith = entity.ReplaceDataWith,
                ReplaceDatesWith = entity.ReplaceDatesWith,
                RemoveCaseAttachments = entity.RemoveCaseAttachments,
                RemoveLogAttachments = entity.RemoveLogAttachments,
                ReplaceEmails = entity.ReplaceEmails
                
            };

            return model;
        }

        private List<string> ConvertToList(string val)
        {
            var res = new List<string>();
            if (!string.IsNullOrEmpty(val))
            {
                var items = val.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);
                if (items != null && items.Any())
                    res.AddRange(items);
            }
            return res;
        }
    }
}