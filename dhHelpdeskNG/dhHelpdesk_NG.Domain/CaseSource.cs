namespace DH.Helpdesk.Domain
{
    public class CaseSource
    {
        public int Id { get; set; }

        public int Customer_Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Dummy for fetching case sources for specified customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public static CaseSource[] GetSources(int customerId)
        {
            return new[] { new CaseSource() { Id = 1, Customer_Id = customerId, Name = "Portal" } };
        }
    }
}
