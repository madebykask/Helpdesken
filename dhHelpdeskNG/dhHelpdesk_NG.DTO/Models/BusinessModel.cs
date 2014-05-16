namespace DH.Helpdesk.BusinessData.Models
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class BusinessModel : INewBusinessModel
    {
        protected BusinessModel()
        {
        }

        protected BusinessModel(ModelStates state)
        {
            this.State = state;
        }

        [IsId]
        public int Id { get; set; }

        public ModelStates State { get; protected set; }
    }
}