namespace DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers
{
    using DH.Helpdesk.Web.Areas.Licenses.Models.Common;

    public sealed class ManufacturersIndexModel : BaseIndexModel
    {
        public override EntityModelType Type
        {
            get
            {
                return EntityModelType.Manufacturers;
            }
        }

        public ManufacturersFilterModel GetFilter()
        {
            return new ManufacturersFilterModel();
        }
    }
}