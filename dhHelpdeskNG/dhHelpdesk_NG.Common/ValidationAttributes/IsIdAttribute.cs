namespace DH.Helpdesk.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class IsIdAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            var value = (int?)args.Value;
            if (value == null)
            {
                return;
            }

            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(args.LocationName, "Must be more than zero.");
            }
        }
    }
}
