﻿namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Domain;

    public static class LicenseFileMapper
    {
        public static LicenseFileModel[] MapToBusinessModels(this IEnumerable<LicenseFile> query)
        {
            var entities = query.Select(f => new
                                    {
                                        f.Id,
                                        f.License_Id,
                                        f.FileName,
                                        f.CreatedDate
                                    })
                                    .OrderBy(f => f.FileName)
                                    .ToArray();

            var models = entities.Select(f => new LicenseFileModel(
                                        f.Id,
                                        f.License_Id, 
                                        f.FileName, 
                                        f.CreatedDate)).ToArray();

            return models;
        }

        public static void MapToEntity(LicenseFileModel model, LicenseFile entity)
        {
            entity.License_Id = model.LicenseId;
            entity.FileName = model.FileName;
        }
    }
}