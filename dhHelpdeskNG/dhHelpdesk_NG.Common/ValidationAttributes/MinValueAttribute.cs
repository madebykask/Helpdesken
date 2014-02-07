namespace DH.Helpdesk.Common.ValidationAttributes
{
    using System;

    using PostSharp.Aspects;

    [Serializable]
    public sealed class MinValueAttribute : LocationInterceptionAspect
    {
        private readonly int minValue;

        public MinValueAttribute(int minValue)
        {
            this.minValue = minValue;
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            base.OnSetValue(args);

            if (args.Value == null)
            {
                return;
            }

            var value = (int)args.Value;
            if (value < this.minValue)
            {
                throw new ArgumentOutOfRangeException(args.LocationName, "Must be more than " + this.minValue + ".");
            }
        }
    }
}
