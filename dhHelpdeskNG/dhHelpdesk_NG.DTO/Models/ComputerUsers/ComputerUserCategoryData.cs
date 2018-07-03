namespace DH.Helpdesk.BusinessData.Models.ComputerUsers
{
    public class ComputerUserCategoryData
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsEmpty { get; set; }
        public bool DefaultInitiatorCategory { get; set; }
        public bool DefaultRegardingCategory { get; set; }
    }
}