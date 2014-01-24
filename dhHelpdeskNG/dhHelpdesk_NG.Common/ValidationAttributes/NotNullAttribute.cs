namespace dhHelpdesk_NG.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class NotNullAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (args.Value == null)
            {
                throw new ArgumentNullException(args.LocationName, "Value cannot be null.");
            }

            base.OnSetValue(args);
        }
    }
}
