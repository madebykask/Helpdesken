namespace dhHelpdesk_NG.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class NotNullAndEmptyAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            if (string.IsNullOrEmpty((string)args.Value))
            {
                throw new ArgumentNullException(args.LocationName, "Value cannot be null or empty.");
            }
        }
    }
}
