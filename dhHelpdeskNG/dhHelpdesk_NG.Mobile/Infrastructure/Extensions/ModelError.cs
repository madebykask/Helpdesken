namespace DH.Helpdesk.Mobile.Infrastructure.Extensions
{
    using System.Collections.Generic;

    public sealed class ModelError
    {
        public ModelError(
                string field, 
                string value, 
                IEnumerable<string> errors)
        {
            this.Errors = errors;
            this.Value = value;
            this.Field = field;
        }

        public string Field { get; private set; }

        public string Value { get; private set; }

        public IEnumerable<string> Errors { get; private set; } 
    }
}