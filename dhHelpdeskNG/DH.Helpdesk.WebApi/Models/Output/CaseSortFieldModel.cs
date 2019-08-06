namespace DH.Helpdesk.WebApi.Models.Output
{
    public class CaseSortFieldModel
    {
        public CaseSortFieldModel()
        {
        }

        public CaseSortFieldModel(string text, string fieldId)
        {
            Text = text;
            FieldId = fieldId;
        }

        public string Text { get; set; }
        public string FieldId { get; set; }
    }
}