namespace DH.Helpdesk.BusinessData.Attributes
{
    using System;

    using DH.Helpdesk.BusinessData.Models;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AllowReadAttribute : LocationInterceptionAspect
    {
        private readonly ModelStates state;

        public AllowReadAttribute(ModelStates state)
        {
            this.state = state;
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            base.OnGetValue(args);

            var businessModel = (BusinessModel)args.Instance;
//            if ((this.state & businessModel.State) != 0)
//            {
//                throw new InvalidOperationException("Not allowed to read the value in the current state of the object.");
//            }
        }
    }
}