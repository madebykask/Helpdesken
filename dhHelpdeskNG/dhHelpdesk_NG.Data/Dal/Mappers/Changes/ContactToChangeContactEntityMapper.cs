namespace DH.Helpdesk.Dal.Dal.Mappers.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ContactToChangeContactEntityMapper : INewBusinessModelToEntityMapper<Contact, ChangeContactEntity>
    {
        public ChangeContactEntity Map(Contact businessModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
