namespace DH.Helpdesk.Services
{
    using System;

    public interface INewEntityIdProvider
    {
        string CreateKey();
    }

    public sealed class NewEntityIdProvider : INewEntityIdProvider
    {
        public string CreateKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
