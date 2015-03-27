namespace DH.Helpdesk.BusinessData.Models.Grid
{
    using System.Collections.Generic;

    /// <summary>
    /// This class holds users settings for each grid for each customer
    /// </summary>
    public class GridSettingsModel
    {
        public readonly string GridId;

        public readonly Dictionary<string, string> Parameters;

        public GridSettingsModel(string gridId, Dictionary<string, string> parameters)
        {
            this.GridId = gridId;
            this.Parameters = parameters;
        }
    }
}
