namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Vendors;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Vendors;

    public sealed class VendorsModelFactory : IVendorsModelFactory
    {
        public VendorsIndexModel GetIndexModel(VendorsFilterModel filter)
        {
            return new VendorsIndexModel();
        }

        public VendorsContentModel GetContentModel(VendorOverview[] vendors)
        {
            return new VendorsContentModel(vendors);
        }

        public VendorEditModel GetEditModel(VendorData data)
        {
            return new VendorEditModel(
                            data.Vendor.Id,
                            data.Vendor.CustomerId,
                            data.Vendor.VendorName,
                            data.Vendor.Contact,
                            data.Vendor.Address,
                            data.Vendor.PostalCode,
                            data.Vendor.PostalAddress,
                            data.Vendor.Phone,
                            data.Vendor.Email,
                            data.Vendor.HomePage);
        }

        public VendorModel GetBusinessModel(VendorEditModel editModel)
        {
            var model = new VendorModel(
                            editModel.Id,
                            editModel.VendorName,
                            editModel.Contact,
                            editModel.Address,
                            editModel.PostalCode,
                            editModel.PostalAddress,
                            editModel.Phone,
                            editModel.Email,
                            editModel.HomePage,
                            editModel.CustomerId);

            return model;
        }
    }
}