﻿namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Licenses;
    using DH.Helpdesk.Services.Services.Licenses;

    public class LicensesService : ILicensesService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IRegionService _regionService;

        private readonly IDepartmentService _departmentService;

        public LicensesService(IUnitOfWorkFactory unitOfWorkFactory,
                               IRegionService regionService,
                               IDepartmentService departmentService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this._regionService = regionService;
            this._departmentService = departmentService;
        }

        public LicenseOverview[] GetLicenses(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                var departmentRepository = uow.GetRepository<Department>();

                var overviews = licenseRepository.GetAll()
                                .GetCustomerLicenses(customerId)
                                .MapToOverviews(departmentRepository.GetAll());                                

                return overviews;
            }
        }

        public LicenseData GetLicenseData(int customerId, int? licenseId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                var productRepository = uow.GetRepository<Product>();
                var regionRepository = uow.GetRepository<Region>();
                var departmentRepository = uow.GetRepository<Department>();
                var vendorRepository = uow.GetRepository<Vendor>();

                LicenseModel license;
                if (licenseId.HasValue)
                {
                    license = licenseRepository.GetAll().MapToBusinessModel(licenseId.Value);
                }
                else
                {
                    license = LicenseModel.CreateDefault();
                }

                var products = productRepository.GetAll()
                                .GetByCustomer(customerId);
                

                var regionId = (license.RegionId.HasValue && license.RegionId.Value > 0)? license.RegionId : null;
                var regions = this._regionService.GetAllRegions()
                                                 .Where(r => r.Customer_Id == customerId && r.IsActive != 0)
                                                 .ToList();                                                                         
                
                var departments = this._departmentService.GetActiveDepartmentsBy(customerId, regionId).ToList();

                var vendors = vendorRepository.GetAll()
                                .GetByCustomer(customerId);

                var upgradeLicenses = licenseRepository.GetAll()
                                .GetUpgradeLicenses(licenseId);

                return LicenseMapper.MapToData(
                                    license,
                                    products,
                                    regions,
                                    departments,
                                    vendors,
                                    upgradeLicenses);
            }
        }

        public LicenseModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                return licenseRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(LicenseModel license)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                var licenseFileRepository = uow.GetRepository<LicenseFile>();

                License entity;
                var now = DateTime.Now;
                if (license.IsNew())
                {
                    entity = new License();
                    LicenseMapper.MapToEntity(license, entity);
                    entity.CreatedDate = now;
                    entity.ChangedDate = now;
                    licenseRepository.Add(entity);
                }
                else
                {
                    entity = licenseRepository.GetById(license.Id);
                    LicenseMapper.MapToEntity(license, entity);
                    entity.ChangedDate = now;
                    licenseRepository.Update(entity);
                }

                foreach (var file in license.Files)
                {
                    LicenseFile fileEntity;
                    if (file.ForDelete)
                    {
                        fileEntity = licenseFileRepository.GetAll()
                                        .GetLicenseFile(entity.Id, file.FileName)
                                        .FirstOrDefault();
                        if (fileEntity != null)
                        {
                            licenseFileRepository.DeleteById(fileEntity.Id);
                        }

                        continue;
                    }

                    fileEntity = new LicenseFile();
                    LicenseFileMapper.MapToEntity(file, fileEntity);
                    entity.CreatedDate = now;
                    entity.ChangedDate = now;
                    entity.Files.Add(fileEntity);
                }

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseRepository = uow.GetRepository<License>();
                var licenseFileRepository = uow.GetRepository<LicenseFile>();

                var license = licenseRepository.GetById(id);
                var fileIds = license.Files.Select(f => f.Id).ToArray();
                foreach (var fileId in fileIds)
                {
                    licenseFileRepository.DeleteById(fileId);    
                }

                licenseRepository.DeleteById(id);

                uow.Save();
            }
        }

        public bool FileExists(int licenseId, string fileName)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseFileRepository = uow.GetRepository<LicenseFile>();

                return licenseFileRepository.GetAll()
                        .GetLicenseFile(licenseId, fileName)
                        .Any();
            }
        }

        public List<string> FindFileNamesExcludeSpecified(int licenseId, List<string> excludeFiles)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseFileRepository = uow.GetRepository<LicenseFile>();

                return licenseFileRepository.GetAll()
                        .GetLicenseFileExclude(licenseId, excludeFiles)
                        .Select(f => f.FileName)
                        .ToList();
            }
        }

        public string[] GetLicenseFileNames(int licenseId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var licenseFileRepository = uow.GetRepository<LicenseFile>();

                return licenseFileRepository.GetAll()
                        .GetLicenseFiles(licenseId)
                        .Select(f => f.FileName)
                        .ToArray();
            }
        }
        
        public List<Department> GetDepartmentsFor(int customerId, int? regionId)
        {
            if (regionId.HasValue && regionId.Value == 0)
                regionId = null;

            return this._departmentService.GetActiveDepartmentsBy(customerId, regionId).ToList();            
        }

    }
}