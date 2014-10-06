namespace DH.Helpdesk.Mobile.Models.Case
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;

    public sealed class MyCasesModel
    {
        public MyCasesModel()
        {
        }

        public MyCasesModel(MyCase[] cases)
        {
            this.Cases = cases;
        }

        public MyCase[] Cases { get; private set; }        
    }
}