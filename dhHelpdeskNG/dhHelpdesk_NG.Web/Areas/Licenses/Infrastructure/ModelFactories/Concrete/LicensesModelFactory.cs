namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;
    using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Licenses;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class LicensesModelFactory : ILicensesModelFactory
    {
        private readonly TemporaryIdProvider temporaryIdProvider;

        public LicensesModelFactory(TemporaryIdProvider temporaryIdProvider)
        {
            this.temporaryIdProvider = temporaryIdProvider;
        }

        public LicensesIndexModel GetIndexModel(LicensesFilterModel filter)
        {
            return new LicensesIndexModel();
        }

        public LicensesContentModel GetContentModel(LicenseOverview[] licenses)
        {
            return new LicensesContentModel(licenses);
        }

        public LicenseEditModel GetEditModel(LicenseData data, List<string> fileUploadWhiteList)
        {
            var products = WebMvcHelper.CreateListField(data.Products, data.License.ProductId);
            var regions = WebMvcHelper.CreateListField(data.Regions, data.License.RegionId);
            var departments = WebMvcHelper.CreateListField(data.Departments, data.License.DepartmentId);
            var vendors = WebMvcHelper.CreateListField(data.Vendors, data.License.VendorId);
            var upgradeLicenss = WebMvcHelper.CreateListField(data.UpgradeLicenses, data.License.UpgradeLicenseId);

            var entityId = data.License.IsNew() ? this.temporaryIdProvider.ProvideTemporaryId() : data.License.Id.ToString(CultureInfo.InvariantCulture);
            var files = new AttachedFilesModel(
                            entityId,
                            data.License.Files.Select(f => f.FileName).ToList());

            return new LicenseEditModel(
                                data.License.Id,
                                data.License.LicenseNumber,
                                data.License.NumberOfLicenses,
                                data.License.PurshaseDate,
                                data.License.Price,
                                data.License.PurshaseInfo,
                                data.License.ValidDate,
                                data.License.PriceYear,
                                data.License.Info,
                                products, 
                                regions,
                                departments, 
                                vendors,
                                upgradeLicenss,
                                files,
								fileUploadWhiteList);
        }

        public LicenseModel GetBusinessModel(LicenseEditModel editModel)
        {
            if (editModel.Info == null)
            {
                editModel.Info = string.Empty;
            }

            var files = new List<LicenseFileModel>();
            files.AddRange(editModel.DeletedFiles.Select(f => new LicenseFileModel(editModel.Id, f, true)));
            files.AddRange(editModel.NewFiles.Where(f => !editModel.DeletedFiles.Contains(f.Name)).Select(f => new LicenseFileModel(editModel.Id, f.Name, false)));

            var model = new LicenseModel(
                                editModel.Id,
                                editModel.LicenseNumber,
                                editModel.NumberOfLicenses,
                                editModel.PurchaseDate,
                                editModel.Price,
                                editModel.PurchaseInfo,
                                editModel.PriceYear,
                                editModel.ProductId,
                                editModel.VendorId,
                                editModel.RegionId,
                                editModel.DepartmentId,
                                editModel.UpgradeLicenseId,
                                editModel.ValidDate,
                                editModel.Info,
                                files.ToArray());

            return model;
        }
    }
}