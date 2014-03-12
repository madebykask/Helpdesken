namespace DH.Helpdesk.BusinessData.Models.Common
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class BusinessModel : INewBusinessModel
    {
        protected BusinessModel(BusinessModelStates businessModelState)
        {
            this.BusinessModelState = businessModelState;
        }

        [IsId]
        public int Id { get; set; }

        public BusinessModelStates BusinessModelState { get; private set; }
    }
}