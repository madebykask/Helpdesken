namespace DH.Helpdesk.BusinessData.Models.Accounts.Read
{
    using System.Collections.Generic;

    public class ProgramForRead : Program
    {
        public ProgramForRead(string infoProduct, List<int> programs)
            : base(infoProduct)
        {
            this.Programs = programs;
        }

        public List<int> Programs { get; private set; }
    }
}