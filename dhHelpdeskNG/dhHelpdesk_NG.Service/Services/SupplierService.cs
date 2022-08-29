namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Supplier.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ISupplierService
    {
        IList<Supplier> GetSuppliers(int customerId);
        IList<Supplier> GetActiveSuppliers(int customerId);
        IList<Supplier> GetSuppliersByCountry(int customerId, int countryId);
        int? GetDefaultId(int customerId); 
        Supplier GetSupplier(int id);
        DeleteMessage DeleteSupplier(int id);

        void SaveSupplier(Supplier supplier, out IDictionary<string, string> errors);
        void Commit();

        /// <summary>
        /// The get supplier overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="SupplierOverview"/>.
        /// </returns>
        SupplierOverview GetSupplierOverview(int id);
    }

    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public SupplierService(
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork)
        {
            this._supplierRepository = supplierRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Supplier> GetSuppliers(int customerId)
        {
            return this._supplierRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<Supplier> GetActiveSuppliers(int customerId)
        {
            return this._supplierRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public IList<Supplier> GetSuppliersByCountry(int customerId, int countryId)
        {
            return this._supplierRepository.GetMany(x => x.Customer_Id == customerId && x.Country_Id == countryId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public int? GetDefaultId(int customerId)
        {
            var r = this._supplierRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }

        public Supplier GetSupplier(int id)
        {
            return this._supplierRepository.Get(x => x.Id == id);
        }
        
        public DeleteMessage DeleteSupplier(int id)
        {
            var supplier = this._supplierRepository.GetById(id);

            if (supplier != null)
            {
                try
                {
                    this._supplierRepository.Delete(supplier);
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
            supplier.ChangedDate = DateTime.UtcNow;
            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(supplier.Name))
                errors.Add("Supplier.Name", "Du måste ange en leverantör");

            if (supplier.IsDefault == 1)
                this._supplierRepository.ResetDefault(supplier.Id, supplier.Customer_Id);

            if (supplier.Id == 0)
                this._supplierRepository.Add(supplier);
            else
                this._supplierRepository.Update(supplier);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// The get supplier overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="SupplierOverview"/>.
        /// </returns>
        public SupplierOverview GetSupplierOverview(int id)
        {
            return this._supplierRepository.GetSupplierOverview(id);
        }
    }
}
