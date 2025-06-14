﻿namespace DH.Helpdesk.Services.Services.Concrete.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.Services.Licenses;

    public class VendorsService : IVendorsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public VendorsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public VendorOverview[] GetVendors(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();

                var overviews = vendorRepository.GetAll()
                                .GetByCustomer(customerId)
                                .MapToOverviews();

                return overviews;
            }
        }

        public VendorData GetVendorData(int customerId, int? vendorId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();

                VendorModel vendor;
                if (vendorId.HasValue)
                {
                    vendor = vendorRepository.GetAll().MapToBusinessModel(vendorId.Value);
                }
                else
                {
                    vendor = VendorModel.CreateDefault(customerId);
                }

                return new VendorData(vendor);
            }
        }

        public VendorModel GetById(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();

                return vendorRepository.GetAll().MapToBusinessModel(id);
            }
        }

        public int AddOrUpdate(VendorModel vendor)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var vendorRepository = uow.GetRepository<Vendor>();
                Vendor entity;
                if (vendor.IsNew())
                {
                    entity = new Vendor();
                    VendorMapper.MapToEntity(vendor, entity);
                    entity.CreatedDate = DateTime.Now;
                    entity.ChangedDate = DateTime.Now;
                    vendorRepository.Add(entity);
                }
                else
                {
                    entity = vendorRepository.GetById(vendor.Id);
                    VendorMapper.MapToEntity(vendor, entity);
                    entity.ChangedDate = DateTime.Now;
                    vendorRepository.Update(entity);
                }

                uow.Save();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Vendor>();

                repository.DeleteById(id);

                uow.Save();
            }
        }
    }
}