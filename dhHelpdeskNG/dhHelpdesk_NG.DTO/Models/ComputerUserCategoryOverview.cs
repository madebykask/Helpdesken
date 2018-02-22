using System;

namespace DH.Helpdesk.BusinessData.Models
{
    public class ComputerUserCategoryOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ComputerUsersCategoryGuid { get; set; }
        public int CustomerId { get; set; }
        public bool IsReadOnly { get; set; }
    }
}