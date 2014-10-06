namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class CurrentTabAttribute : OnMethodBoundaryAspect
    {
        private readonly string tabName;

        public CurrentTabAttribute(string tabName)
        {
            this.tabName = tabName;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            SessionFacade.ActiveTab = this.tabName;
        }
    }
}