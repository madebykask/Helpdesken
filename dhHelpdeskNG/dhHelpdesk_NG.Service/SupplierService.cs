using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ISupplierService
    {
        IList<Supplier> GetSuppliers(int customerId);
        IList<Supplier> GetSuppliersByCountry(int customerId, int countryId);
        int? GetDefaultId(int customerId); 
        Supplier GetSupplier(int id);
        DeleteMessage DeleteSupplier(int id);

        void SaveSupplier(Supplier supplier, out IDictionary<string, string> errors);
        void Commit();
    }

    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Supplier> GetSuppliers(int customerId)
        {
            return _supplierRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<Supplier> GetSuppliersByCountry(int customerId, int countryId)
        {
            return _supplierRepository.GetMany(x => x.Customer_Id == customerId && x.Country_Id == countryId).OrderBy(x => x.Name).ToList();
        }

        public int? GetDefaultId(int customerId)
        {
            var r = _supplierRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }

        public Supplier GetSupplier(int id)
        {
            return _supplierRepository.Get(x => x.Id == id);
        }
        
        public DeleteMessage DeleteSupplier(int id)
        {
            var supplier = _supplierRepository.GetById(id);

            if (supplier != null)
            {
                try
                {
                    _supplierRepository.Delete(supplier);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveSupplier(Supplier supplier, out IDictionary<string, string> errors)
        {
            if (supplier == null)
                throw new ArgumentNullException("supplier");

            supplier.SupplierNumber = supplier.SupplierNumber ?? "";
            
            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(supplier.Name))
                errors.Add("Supplier.Name", "Du måste ange en leverantör");

            if (supplier.Id == 0)
                _supplierRepository.Add(supplier);
            else
                _supplierRepository.Update(supplier);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
