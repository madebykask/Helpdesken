namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using System.Collections.Generic;

    public class ProgramForWrite : Program
    {
        public ProgramForWrite(string infoProduct, List<int> programIds)
            : base(infoProduct)
        {
            this.ProgramIds = programIds;
        }

        public List<int> ProgramIds { get; private set; }
    }
}