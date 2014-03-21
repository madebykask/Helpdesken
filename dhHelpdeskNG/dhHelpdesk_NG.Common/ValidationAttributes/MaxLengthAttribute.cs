namespace DH.Helpdesk.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MaxLengthAttribute : LocationInterceptionAspect
    {
        private readonly int maxLength;

        public MaxLengthAttribute(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            if (args.Value == null)
            {
                return;
            }

            if (((string)args.Value).Length > maxLength)
            {
                throw new ArgumentOutOfRangeException(
                    args.LocationName,
                    "Max string length is " + maxLength + " character(s).");
            }
        }
    }
}