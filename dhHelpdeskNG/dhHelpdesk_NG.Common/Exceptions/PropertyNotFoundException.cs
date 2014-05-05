namespace DH.Helpdesk.Common.Exceptions
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class PropertyNotFoundException : Exception
    {
        #region Constructors and Destructors

        public PropertyNotFoundException(string message, string propertyName)
            : base(message)
        {
            this.PropertyName = propertyName;
        }

        #endregion

        #region Public Properties

        [NotNullAndEmpty]
        public string PropertyName { get; private set; }

        #endregion
    }
}