namespace DH.Helpdesk.Common.Collections
{
    using System;
    using System.Collections.Generic;

    public sealed class NamedObjectCollection<TValue> : List<TValue> where TValue : INamedObject
    {
        public NamedObjectCollection(List<TValue> items) : base(items)
        {
        }

        public TValue FindByName(string name)
        {
            return this.Find(s => s.GetName().Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
