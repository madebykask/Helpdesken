﻿namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public static class ApplicationMapper
    {
        public static ApplicationOverview[] MapToOverviews(
                            this IQueryable<Application> query, 
                            IQueryable<Software> software,
                            IQueryable<Computer> computers)
        {            
            var entities = query.Select(a => new 
                                            {
                                                ApplicationId = a.Id,
                                                ApplicationName = a.Name,
                                                a.Products,
                                                NumberOfInstallations = 
                                                    computers.Count(c => software.Where(s => a.Name == s.Name).Select(s => s.Computer_Id).Contains(c.Id))
                                            })
                                            .OrderBy(a => a.ApplicationName)
                                            .ToArray();

            var overviews = entities.Select(a =>
                {
                    var product = a.Products.FirstOrDefault();
                    return new ApplicationOverview(
                        a.ApplicationId,
                        a.ApplicationName,
                        product != null ? product.Name : null,
                        a.NumberOfInstallations);
                }).ToArray();

            return overviews;
        }

        public static ApplicationModel MapToBusinessModel(this IQueryable<Application> query, int id)
        {
            ApplicationModel model = null;

            var entity = query.GetById(id)
                        .Select(a => new
                                {
                                    a.Id,
                                    a.Customer_Id,
                                    a.Name,
                                    a.CreatedDate,
                                    a.ChangedDate,
                                    a.Products,
                                }).SingleOrDefault();

            if (entity != null)
            {
                var product = entity.Products.SingleOrDefault();
                model = new ApplicationModel(
                                    entity.Id,
                                    entity.Customer_Id,
                                    entity.Name,
                                    product != null ? product.Id : (int?)null,
                                    entity.CreatedDate,
                                    entity.ChangedDate);
            }

            return model;
        }

        public static void MapToEntity(ApplicationModel model, Application entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.Name = model.ApplicationName;
        }

        public static ItemOverview[] MapToOverviews(IEnumerable<Application> applications)
        {
            if (applications == null)
            {
                return null;
            }

            return applications.Select(a => new ItemOverview(a.Name, a.Id.ToString(CultureInfo.InvariantCulture))).ToArray();
        }
    }
}