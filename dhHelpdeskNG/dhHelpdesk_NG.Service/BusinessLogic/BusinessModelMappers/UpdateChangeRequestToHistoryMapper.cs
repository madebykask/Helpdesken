namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers
{
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.Services.Requests.Changes;

    public sealed class UpdateChangeRequestToHistoryMapper : IBusinessModelsMapper<UpdateChangeRequest, History>
    {
        public History Map(UpdateChangeRequest businessModel)
        {
            return History.CreateNew(
                businessModel.Change.Id,
                businessModel.Change.Orderer,
                businessModel.Change.General,
                businessModel.Change.Registration,
                businessModel.Change.Analyze,
                businessModel.Change.Implementation,
                businessModel.Change.Evaluation,
                businessModel.Context.UserId,
                businessModel.Context.DateAndTime);
        }
    }
}