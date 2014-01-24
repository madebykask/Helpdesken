namespace dhHelpdesk_NG.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class NotNullAndEmptyArrayAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            if (args.Value == null || ((Array)args.Value).Length == 0)
            {
                throw new ArgumentNullException(args.LocationName, "Value cannot be null or empty.");
            }
        }
    }
}
