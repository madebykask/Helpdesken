namespace DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;

    public sealed class ManufacturerEditModel
    {
        public ManufacturerEditModel(ManufacturerData data)
        {
            this.Data = data;
        }

        public ManufacturerEditModel()
        {            
        }

        public ManufacturerData Data { get; set; }
    }
}