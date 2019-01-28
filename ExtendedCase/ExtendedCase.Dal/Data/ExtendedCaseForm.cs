
namespace ExtendedCase.Dal.Data
{
    public class ExtendedCaseForm : EntityBase
    {
        public int Id { get; set; }
        public string MetaData { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DefaultLanguageId { get; set; }
    }
}
