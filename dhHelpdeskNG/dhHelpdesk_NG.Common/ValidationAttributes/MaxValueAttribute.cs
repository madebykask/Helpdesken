namespace dhHelpdesk_NG.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class MaxValueAttribute : LocationInterceptionAspect
    {
        private readonly int maxValue;

        public MaxValueAttribute(int maxValue)
        {
            this.maxValue = maxValue;
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            if (args.Value == null)
            {
                return;
            }

            var value = (int)args.Value;
            if (value > this.maxValue)
            {
                throw new ArgumentOutOfRangeException(args.LocationName, "Must be less than " + this.maxValue + ".");
            }
        }
    }
}