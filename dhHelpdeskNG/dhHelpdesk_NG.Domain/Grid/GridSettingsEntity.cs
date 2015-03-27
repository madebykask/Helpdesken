namespace DH.Helpdesk.Domain.Grid
{
    public class GridSettingsEntity: Entity
    {
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// ID of the grid, for ex. case_overview
        /// </summary>
        public string GridId { get; set; }

        /// <summary>
        /// Name of the paramter, ex. limit
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Value of the paramter, ex. 50
        /// </summary>
        public string Value { get; set; }
    }
}