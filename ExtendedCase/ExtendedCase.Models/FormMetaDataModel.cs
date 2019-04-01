namespace ExtendedCase.Models
{
    public class FormMetaDataModel
    {
        public FormMetaDataModel()
        {
        }

        public FormMetaDataModel(int id, string metaData)
        {
            Id = id;
            MetaData = metaData;
        }

        public int Id { get; set; }
        public string MetaData { get; set; }
    }
}