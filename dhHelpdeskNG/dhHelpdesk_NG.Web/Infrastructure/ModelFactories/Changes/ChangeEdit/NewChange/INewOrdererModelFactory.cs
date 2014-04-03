namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface INewOrdererModelFactory
    {
        OrdererModel Create(ChangeEditData editData, OrdererEditSettings settings);
    }
}