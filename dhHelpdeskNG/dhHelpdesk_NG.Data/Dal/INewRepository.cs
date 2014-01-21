namespace dhHelpdesk_NG.Data.Dal
{
    using dhHelpdesk_NG.DTO.DTOs;

    public interface INewRepository<TNewBusinessModel, TUpdatedBusinessModel>
        where TNewBusinessModel : IBusinessModelWithId where TUpdatedBusinessModel : IBusinessModelWithId
    {
        void DeleteById(int id);

        void Add(TNewBusinessModel businessModel);

        void Update(TUpdatedBusinessModel businessModel);
    }
}
