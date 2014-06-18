namespace DH.Helpdesk.Services.BusinessLogic.MailTools
{
    using System.Collections.Generic;

    public sealed class EmailMarkValues
    {
        private readonly Dictionary<string, string> markValues = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                return this.markValues[key];
            }
        }

        public void Add(string key, string value)
        {
            if (!this.ContainsKey(key))
            {
                this.markValues.Add(key, value);
            }
        }

        public bool ContainsKey(string key)
        {
            return this.markValues.ContainsKey(key);
        }
    }
}