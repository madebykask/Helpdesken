namespace DH.Helpdesk.Web.Models.Case
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

        public MyCase[] Cases { get; set; }
        public bool ShowMore { get; set; }
    }
}