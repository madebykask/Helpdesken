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


            var entities = query.Select(p => new
                        {
                            ProductId = p.Id,
                            ProductName = p.Name,
                            Licenses = p.Licenses,
                            Regions = p.Licenses.Where(l => (regionsFilter.Any() ? regionsFilter.Contains(l.Region_Id.Value) : true))
                                                            .Select(l => new { Id = l.Id, Name = l.Region })
                                                            .Distinct(),

                            //Departments = p.Licenses.Where(l => (departmentsFilter.Any() ? departmentsFilter.Contains(l.Department_Id.Value) : true))
                            //                                    .Select(l => new { Id = l.Department.Id, Name = l.Department.DepartmentName })
                            //                                    .Distinct(),
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
                    RegionId = (c.Region_Id != null) ? c.Region_Id.Value : 0,
                    Region = c.Region,
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
                        //Another region
                        int howManyInUseWithAnotherRegion = 0;
                        var compswithanotherreg = usedLicenses.Where(d => d.RegionId != l.Region_Id && d.RegionId != 0 && d.DepartmentId == 0).GroupBy(c => c.RegionId).ToList();
                        if (compswithanotherreg.Count > 0)
                        {
                            foreach (var group in compswithanotherreg)
                            {
                                
                                string depName = "";
                                string regName = "";
                                int depId = 0;
                                int regionId = 0;
                                foreach (var c in group)
                                {
                                    if (c.RegionId != 0)
                                    {
                                        regionId = c.RegionId;
                                        regName = c.Region.Name;
                                    }

                                    howManyInUseWithAnotherRegion++;

                                }
                                curLicense = new ProductLicense(regionId, depId, regName, depName, 0, group.Count());
                                curProduct.ProductLicenses.Add(curLicense);
                            }

                        }
                        //Another department
                        int howManyInUseWithAnotherDep = 0;
                        var compswithanotherdep = usedLicenses.Where(d => d.DepartmentId != l.Department_Id && d.DepartmentId != 0 && d.DepartmentId == 0).GroupBy(c => c.RegionId).ToList();
                        if (compswithanotherdep.Count > 0)
                        {
                            foreach (var group in compswithanotherdep)
                            {

                                string depName = "";
                                string regName = "";
                                int depId = 0;
                                int regionId = 0;
                                foreach (var c in group)
                                {
                                    if (c.RegionId != 0)
                                    {
                                        depId = c.RegionId;
                                        depName = c.Region.Name;
                                    }

                                    howManyInUseWithAnotherDep++;

                                }
                                curLicense = new ProductLicense(regionId, depId, regName, depName, 0, group.Count());
                                curProduct.ProductLicenses.Add(curLicense);
                            }

                        }
                        //Without region and department
                        var howManyInUse = usedLicenses.Where(p => p.DepartmentId == 0).Count() - howManyInUseWithAnotherRegion - howManyInUseWithAnotherDep;
                        curLicense = new ProductLicense(null, null, "", "", l.NumberOfLicenses, howManyInUse);
                        curProduct.ProductLicenses.Add(curLicense);
                    }

                    if (l.Department_Id.HasValue && !l.Region_Id.HasValue)
                    {
                        var compswithanotherdep = usedLicenses.Where(d => d.DepartmentId != l.Department_Id && d.DepartmentId != 0).GroupBy(c => c.DepartmentId).ToList();
                        if (compswithanotherdep.Count > 0)
                        {
                            foreach (var group in compswithanotherdep)
                            {
                                int howMany = 0;
                                string depName = "";
                                string regName = "";
                                int depId = 0;
                                int regionId = 0;
                                foreach (var c in group)
                                {
                                    if (c.RegionId != 0)
                                    {
                                        regionId = c.RegionId;
                                        regName = c.Region.Name;
                                    }
                                        
                                    howMany++;
                                    depName = c.DepartmentName;
                                    depId = c.DepartmentId;

                                }
                                curLicense = new ProductLicense(regionId, depId, regName, depName, 0, group.Count());
                                curProduct.ProductLicenses.Add(curLicense);
                            }

                        }
                        var compswithsamedep = usedLicenses.Where(d => d.DepartmentId == l.Department_Id).Count();
                        if (compswithsamedep > 0)
                        {
                            curLicense = new ProductLicense(null, l.Department_Id, "", l.Department.DepartmentName, l.NumberOfLicenses, compswithsamedep);
                            curProduct.ProductLicenses.Add(curLicense);
                        }
                    }

                    if (l.Region_Id.HasValue && !l.Department_Id.HasValue)
                    {
                        var compswithanotherregion = usedLicenses.Where(d => d.RegionId != l.Region_Id && d.RegionId != 0).GroupBy(c => c.RegionId).ToList(); 
                        if (compswithanotherregion.Count > 0)
                        {
                            foreach (var group in compswithanotherregion)
                            {
                                int howMany = 0;
                                string regionName = "";
                                int regionId = 0;
                                foreach (var c in group)
                                {
                                    howMany++;
                                    regionName = c.Region.Name;
                                    regionId = c.RegionId;

                                }
                                curLicense = new ProductLicense(regionId, null, regionName, "", 0, group.Count());
                                curProduct.ProductLicenses.Add(curLicense);
                            }
                        }
                        var compswithsamereg = usedLicenses.Where(d => d.RegionId == l.Region_Id).Count();
                        if (compswithsamereg > 0)
                        {
                            curLicense = new ProductLicense(l.Region_Id, null, l.Region.Name, "", l.NumberOfLicenses, compswithsamereg);
                            curProduct.ProductLicenses.Add(curLicense);
                        }
                    }
                    if (l.Region_Id.HasValue && l.Department_Id.HasValue)
                    {
                        //Hope this works
                        //Same region and department
                        var howManyInUse = usedLicenses.Where(p => p.DepartmentId == l.Department_Id).Count();
                        curLicense = new ProductLicense(l.Region_Id, l.Department_Id, l.Region.Name, l.Department.DepartmentName, l.NumberOfLicenses, howManyInUse);
                        curProduct.ProductLicenses.Add(curLicense);

                        var compswithnodep = usedLicenses.Where(d => d.DepartmentId != l.Department_Id).GroupBy(c => c.DepartmentId).ToList();
                        if (compswithnodep.Count > 0)
                        {
                            foreach (var group in compswithnodep)
                            {
                                int howMany = 0;
                                string depName = "";
                                string regName = "";
                                int depId = 0;
                                int regionId = 0;
                                foreach (var c in group)
                                {
                                    if (c.RegionId != 0)
                                    {
                                        regionId = c.RegionId;
                                        regName = c.Region.Name;
                                    }

                                    howMany++;
                                    depName = c.DepartmentName;
                                    depId = c.DepartmentId;

                                }
                                curLicense = new ProductLicense(regionId, depId, regName, depName, 0, group.Count());
                                curProduct.ProductLicenses.Add(curLicense);
                            }

                        }


                    }

                    //var alreadyExists = curProduct.ProductLicenses.Where(p => p.DepartmentId == curLicense.DepartmentId).SingleOrDefault();
                    //if (alreadyExists != null)
                    //    alreadyExists.NumberOfLicenses += curLicense.NumberOfLicenses;
                    //else
                    //    curProduct.ProductLicenses.Add(curLicense);
                }

                if (e.Licenses.Count == 0)
                {
                    curLicense = new ProductLicense(null, null, "", "", 0, 0);
                    curProduct.ProductLicenses.Add(curLicense);
                }
                overviews.Add(curProduct);
            }


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