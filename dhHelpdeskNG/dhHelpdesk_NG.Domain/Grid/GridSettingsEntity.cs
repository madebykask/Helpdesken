namespace DH.Helpdesk.Domain.Grid
{
    /// <summary>
    /// Table for storing column settings.
    /// 
    /// </summary>
    public class GridSettingsEntity : Entity
    {
        public int CustomerId { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// ID of the grid, for ex. case_overview
        /// </summary>
        public int GridId { get; set; }

        /// <summary>
        /// ID of the field from DH.Helpdesk.BusionessData.Models.Grid.GridColumnsDefinition
        /// </summary>
        public int? FieldId { get; set; }

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