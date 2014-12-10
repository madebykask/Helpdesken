namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    using System.Collections.Generic;

    public class Program
    {
        public Program(string infoProduct, List<int> programIds)
        {
            this.InfoProduct = infoProduct;
            this.ProgramIds = programIds;
        }

        public string InfoProduct { get; private set; }

        public List<int> ProgramIds { get; private set; }

        public static Program CreateDefault()
        {
            return new Program(null, null);
        }
    }
}