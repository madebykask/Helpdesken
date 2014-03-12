namespace DH.Helpdesk.BusinessData.Attributes
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common;

    using PostSharp.Aspects;

    [Serializable]
    public class CanGet : LocationInterceptionAspect
    {
        private readonly BusinessModelStates businessModelState;

        public CanGet(BusinessModelStates businessModelState)
        {
            this.businessModelState = businessModelState;
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            base.OnGetValue(args);

            var businessModel = (BusinessModel)args.Instance;

            if (businessModel.BusinessModelState == BusinessModelStates.ForEdit)
            {
                return;
            }

            if (businessModel.BusinessModelState != this.businessModelState)
            {
                throw new InvalidOperationException("Value can not be given in current state of object");
            }
        }
    }
}