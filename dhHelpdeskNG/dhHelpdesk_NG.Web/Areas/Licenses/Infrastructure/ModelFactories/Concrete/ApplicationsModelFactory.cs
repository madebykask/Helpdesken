namespace DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Applications;
    using DH.Helpdesk.Web.Areas.Licenses.Models.Applications;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class ApplicationsModelFactory : IApplicationsModelFactory
    {
        public ApplicationsIndexModel GetIndexModel(ApplicationsFilterModel filter)
        {
            return new ApplicationsIndexModel(filter.OnlyConnected);
        }

        public ApplicationsContentModel GetContentModel(ApplicationOverview[] applications)
        {
            return new ApplicationsContentModel(applications);
        }

        public ApplicationEditModel GetEditModel(ApplicationData data)
        {
            var products = WebMvcHelper.CreateListField(data.Products);

            return new ApplicationEditModel(
                                data.Application.Id,
                                data.Application.CustomerId,
                                data.Application.ApplicationName,
                                products);
        }

        public ApplicationModel GetBusinessModel(ApplicationEditModel editModel)
        {
            var model = new ApplicationModel(
                                editModel.Id,
                                editModel.CustomerId,
                                editModel.ApplicationName);

            return model;
        }
    }
}