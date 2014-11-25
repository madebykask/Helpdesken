namespace DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview
{
    using System.Collections.Generic;

    public class Program
    {
        public Program(string infoProduct, List<string> programs)
        {
            this.InfoProduct = infoProduct;
            this.Programs = programs;
        }

        public string InfoProduct { get; private set; }

        public List<string> Programs { get; private set; }
    }
}