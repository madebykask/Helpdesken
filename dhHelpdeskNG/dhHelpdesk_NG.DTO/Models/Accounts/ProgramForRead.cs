namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public class ProgramForRead : Program
    {
        public ProgramForRead(string infoProduct, List<ItemOverview> programs)
            : base(infoProduct)
        {
            this.Programs = programs;
        }

        public List<ItemOverview> Programs { get; private set; }
    }
}