namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSolutionCondition : Entity
    {

        public int Customer_Id { get; set; }
        public int Id { get; set; }
        public int IsSelected { get; set; }
        public string Name { get; set; }
        public string StateSecondaryGUID { get; set; }



    }
}
