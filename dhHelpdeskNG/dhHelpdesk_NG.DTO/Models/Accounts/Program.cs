namespace DH.Helpdesk.BusinessData.Models.Accounts
{
    public abstract class Program
    {
        protected Program(string infoProduct)
        {
            this.InfoProduct = infoProduct;
        }

        public string InfoProduct { get; private set; }
    }
}