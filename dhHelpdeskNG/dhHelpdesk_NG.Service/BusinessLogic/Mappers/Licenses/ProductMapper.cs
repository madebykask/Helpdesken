namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses
{
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Products;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using System.Collections.Generic;
    
    public class myproducts
    {
        public int? Key {get; set;}
        public int value {get; set;} 
    }

    public static class ProductMapper
    {
        public class GroupedAvailableLicense
        {
            public GroupedAvailableLicense(Region region, Department dep, int num) 
            {
                this.Region = region;
                this.Department = dep;
                this.Number = num;
            }

            public Region Region { get; set; }
            public Department Department { get; set; }
            public int Number { get; set; }

        }

        public static ProductOverview[] MapToOverviews(
                                this IQueryable<Product> query, 
                                IQueryable<Software> software,
                                IQueryable<Computer> computers,
                                int[] regionsFilter, int[] departmentsFilter)
        {           

            //Regions = p.Licenses.Where(l=> (departmentsFilter.Any()? departmentsFilter.Contains(l.Department_Id.Value):true))
            //                                    .Select(l => l.Region.Name)
            //                                    .Distinct(),

            //                Departments = p.Licenses.Where(l => (departmentsFilter.Any() ? departmentsFilter.Contains(l.Department_Id.Value) : true))
            //                                        .Select(l => new { Id = l.Department.Id, Name = l.Department.DepartmentName })
            //                                        .Distinct(),
            var entities = query.Select(p => new
                        {
                            ProductId = p.Id,
                            ProductName = p.Name,
                            Licenses = p.Licenses,
                            //LicencesNumber = p.Licenses.Where(l => (departmentsFilter.Any() ? departmentsFilter.Contains(l.Department_Id.Value) : true))
                            //                           .GroupBy(ls => ls.Department_Id != null ? ls.Department_Id : 0)
                            //                           .Select(l => new
                            //                                {
                            //                                    DepartmentId = l.Key,                                                                                   
                            //                                    LicensesCount = l.Sum(nl => nl.NumberOfLicenses)
                            //                                })
                            //                           .ToList(),

                            //LicencesNumber2 = p.Licenses.Where(l => (departmentsFilter.Any() ? departmentsFilter.Contains(l.Department_Id.Value) : true))
                            //                           .GroupBy(ls => ls.Department_Id != null ? ls.Department_Id : 0)
                            //                           .Select(l => new
                            //                           {
                            //                               R = l.Key
                            //                               DepartmentId = l.Key,
                            //                               LicensesCount = l.Sum(nl => nl.NumberOfLicenses)
                            //                           })
                            //                           .ToList(),
                                               
                           
                            UsedLicencesNumber = computers.Select(c => new { DepartmentId = (c.Department_Id != null) ? c.Department_Id.Value : 0, 
                                                                             DepartmentName = c.Department.DepartmentName, 
                                                                             ComputerId = c.Id  
                                                                           })
                                                          .Where(c => (departmentsFilter.Any() ? departmentsFilter.Contains(c.DepartmentId) : true))
                                                          .Where(c => software.Where(s => p.Applications.Select(a => a.Name).Contains(s.Name))
                                                                              .Select(s=> s.Computer_Id)
                                                                              .Contains(c.ComputerId))
                                                          .ToList()
                        })
                        .OrderBy(p => p.ProductName)
                        .ToArray();

            //ToDo - Users Departments id or computers id???
            var overviews = new List<ProductOverview>();
            foreach (var e in entities)
            {
                var curProduct = new ProductOverview(e.ProductId, e.ProductName);
                var curLicense = new ProductLicense();
                var usedLicenses = e.UsedLicencesNumber;
                foreach (var l in e.Licenses)
                {
                    

                    if (!l.Region_Id.HasValue && !l.Department_Id.HasValue)
                    {
                        //RegionID?
                        var howManyInUse = usedLicenses.Where(p => p.DepartmentId == 0).Count();
                        curLicense = new ProductLicense(null, null, "", "", l.NumberOfLicenses, howManyInUse);
                    }
                        
                    
                    //if (l.Region_Id.HasValue && l.Department_Id.HasValue)
                    //{
                    //    var howManyInUse = curProduct.ProductLicenses.Where(p => p.DepartmentId == l.Department_Id && p.RegionId == l.Region_Id).SingleOrDefault();
                    //    curLicense = new ProductLicense(l.Region_Id, l.Department_Id, l.Region.Name, l.Department.DepartmentName, l.NumberOfLicenses, howManyInUse.NumberOfLicenses);
                    //}

                    //if (l.Region_Id.HasValue && !l.Department_Id.HasValue)
                    //{
                    //    var howManyInUse = curProduct.ProductLicenses.Where(p => p.RegionId == l.Region_Id).SingleOrDefault();
                    //    curLicense = new ProductLicense(l.Region_Id, null, l.Region.Name, "", l.NumberOfLicenses, howManyInUse.NumberOfLicenses);
                    //}
                        

                    if (!l.Region_Id.HasValue && l.Department_Id.HasValue)
                    {
                        var howManyInUse = usedLicenses.Where(p => p.DepartmentId == l.Department_Id).Count();
                        curLicense = new ProductLicense(null, l.Department_Id, "", l.Department.DepartmentName, l.NumberOfLicenses, howManyInUse);
                    }
                       

                    var alreadyExists = curProduct.ProductLicenses.Where(p => p.DepartmentId == curLicense.DepartmentId).SingleOrDefault();
                    if (alreadyExists != null)
                        alreadyExists.NumberOfLicenses += curLicense.NumberOfLicenses;
                    else
                        curProduct.ProductLicenses.Add(curLicense);
                }

                if (e.Licenses.Count == 0)
                {
                    curLicense = new ProductLicense(null, null, "", "", 0, 0);
                    curProduct.ProductLicenses.Add(curLicense);
                }
                overviews.Add(curProduct);
            }

            //var overviews = entities.Select(p => new ProductOverview(
            //                                        p.ProductId,
            //                                        p.ProductName,
            //                                        p.Regions.ToArray(),
            //                                        p.Departments.Select(d => new KeyValuePair<int, string>(d.Id, d.Name))
            //                                                     .Union(p.UsedLicencesNumber.Where(ul=> !p.Departments.Select(d=> d.Id).Contains(ul.DepartmentId))
            //                                                                                .Select(ul=> new KeyValuePair<int, string>(ul.DepartmentId, ul.DepartmentName))).ToArray(),

            //                                        p.LicencesNumber.Where(ln => ln != null)
            //                                                        .Select(ln => new KeyValuePair<int?,int>(ln.DepartmentId, ln.LicensesCount))
            //                                                        .Union(p.UsedLicencesNumber.Where(ul => !p.LicencesNumber.Select(l => l.DepartmentId).Contains(ul.DepartmentId))
            //                                                                                   .Select(ul => new KeyValuePair<int?, int>(ul.DepartmentId, 0))).ToArray(),

            //                                        p.UsedLicencesNumber.Select(un => new KeyValuePair<int?, int>(un.DepartmentId, un.ComputerId)).ToArray()
            //                                        )).ToArray();

            return overviews.ToArray();
        }

        public static ProductModel MapToBusinessModel(this IQueryable<Product> query, int id)
        {
            ProductModel model = null;

            var entity = query.GetById(id) 
                        .Select(p => new
                            {
                                p.Id,
                                p.Name,
                                p.Manufacturer_Id,
                                p.Customer_Id,
                                p.CreatedDate,
                                p.ChangedDate
                            })
                            .SingleOrDefault();

            if (entity != null)
            {
                model = new ProductModel(
                                entity.Id,
                                entity.Name,
                                entity.Manufacturer_Id,
                                entity.Customer_Id,
                                entity.CreatedDate,
                                entity.ChangedDate);
            }

            return model;
        }

        public static void MapToEntity(ProductModel model, Product entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.Manufacturer_Id = model.ManufacturerId;
            entity.Name = model.ProductName;
        }

        public static ProductsFilterData MapToFilterData(
                        IQueryable<Region> regions,
                        IQueryable<Department> departments,
                        IQueryable<Product> products)
        {
            var reg = regions.Select(r => new { Id = r.Id, Name = r.Name, Type = "Region" }).ToArray();
            var dep = departments.Select(d => new { Id = d.Id, Name = d.DepartmentName, Type = "Department" }).ToArray();
            var prod = products.Select(p => new { Id = p.Id, Name = p.Name, Type = "Product" }).ToArray();
            var overviews = reg.Union(dep).Union(prod)
                               .OrderBy(o => o.Type)
                               .ThenBy(o => o.Name)
                               .ToArray();

            return new ProductsFilterData(
                        overviews.Where(o => o.Type == "Region").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                        overviews.Where(o => o.Type == "Department").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                        overviews.Where(o => o.Type == "Product").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray());
        }

        public static ProductData MapToData(
                        ProductModel product,
                        IQueryable<Manufacturer> manufacturers,
                        IQueryable<Application> availableApplications,
                        IQueryable<Application> applications)
        {
            var overviews = manufacturers.Select(m => new { m.Id, m.Name, Type = "Manufacturer" }).Union(
                            availableApplications.Select(m => new { m.Id, m.Name, Type = "availableApplications" }).Union(
                            applications.Select(a => new { a.Id, a.Name, Type = "Application" })))
                            .OrderBy(o => o.Type)
                            .ThenBy(o => o.Name)
                            .ToArray();

            var applicationsOverviews = overviews.Where(o => o.Type == "Application").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray();
            var availableApplicationsOverviews = overviews
                        .Where(o => o.Type == "availableApplications" && !applicationsOverviews.Any(a => a.Value == o.Id.ToString(CultureInfo.InvariantCulture)))
                        .Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray();

            return new ProductData(
                        product,
                        overviews.Where(o => o.Type == "Manufacturer").Select(o => new ItemOverview(o.Name, o.Id.ToString(CultureInfo.InvariantCulture))).ToArray(),
                        availableApplicationsOverviews,
                        applicationsOverviews);
        }
    }
}