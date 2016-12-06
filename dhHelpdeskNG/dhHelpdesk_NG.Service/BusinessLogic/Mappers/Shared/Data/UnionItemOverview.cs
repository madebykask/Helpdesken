namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Shared.Data
{
    using DH.Helpdesk.Common.ValidationAttributes;

    internal class UnionItemOverview
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        [NotNullAndEmpty]
        public string Type { get; set; }
	}
}