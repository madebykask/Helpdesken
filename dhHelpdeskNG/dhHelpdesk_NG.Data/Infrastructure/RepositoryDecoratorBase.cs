namespace dhHelpdesk_NG.Data.Infrastructure
{
    using System.Data;

    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.DTO.DTOs;

    public abstract class RepositoryDecoratorBase<TDomain, TDto> : RepositoryBase<TDomain>
        where TDomain : Entity
        where TDto : IBusinessModelWithId
    {
        protected RepositoryDecoratorBase(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public abstract TDomain MapFromDto(TDto dto);

        public virtual void Add(TDto dto)
        {
            var entity = this.MapFromDto(dto);
            this.DataContext.Set<TDomain>().Add(entity);
            this.InitializeAfterCommit(dto, entity);
        }

        public virtual void Delete(int id)
        {
            var entity = this.DataContext.Set<TDomain>().Find(id);
            this.DataContext.Set<TDomain>().Remove(entity);
        }

        public virtual void Update(TDto dto)
        {
            var entity = this.MapFromDto(dto);
            this.DataContext.Set<TDomain>().Attach(entity);
            this.DataContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
