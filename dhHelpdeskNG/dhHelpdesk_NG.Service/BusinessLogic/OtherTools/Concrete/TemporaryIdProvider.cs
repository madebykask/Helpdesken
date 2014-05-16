namespace DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete
{
    using System;

    public sealed class TemporaryIdProvider : ITemporaryIdProvider
    {
        public string ProvideTemporaryId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}