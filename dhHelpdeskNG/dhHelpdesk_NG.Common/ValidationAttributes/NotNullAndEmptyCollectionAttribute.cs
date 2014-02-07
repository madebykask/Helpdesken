namespace DH.Helpdesk.Common.ValidationAttributes
{
    using System;
    using System.Collections;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class NotNullAndEmptyCollectionAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            var value = (ICollection)args.Value;
            if (value == null || value.Count == 0)
            {
                throw new ArgumentNullException(args.LocationName, "Value cannot be null or empty.");
            }
        }
    }
}
