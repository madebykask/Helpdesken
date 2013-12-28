namespace dhHelpdesk_NG.Web.Models.Problem.Input
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using dhHelpdesk_NG.Web.Models.Problem.Output;

    public class ProblemEditInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int ProblemNumber { get; set; }

        public string Description { get; set; }

        public int? ResponsibleUserId { get; set; }

        [Required]
        [StringLength(20)]
        public string InventoryNumber { get; set; }

        public bool ShowOnStartPage { get; set; }

        public List<LogOutputModel> Logs { get; set; }

        public List<CaseOutputModel> Cases { get; set; }
    }
}