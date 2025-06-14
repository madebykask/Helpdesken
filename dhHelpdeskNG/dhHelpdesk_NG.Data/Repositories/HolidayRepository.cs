// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HolidayRepository.cs" company="">
//   IHolidayRepository
// </copyright>
// <summary>
//   Interface for access to holidays.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region HOLIDAY

    /// <summary>
    /// Interface for access to holidays.
    /// </summary>
    public interface IHolidayRepository : IRepository<Holiday>
    {
        IEnumerable<Holiday> GetHolidaysByHeaderId(int id);
        IEnumerable<Holiday> GetHolidaysByHeaderIdAndYear(int year, int id);
        IList<Holiday> GetHolidaysByHeaderIdAndYearForList(int year, int id);
    }

    /// <summary>
    /// The HolidayHeaderRepository interface.
    /// </summary>
    public interface IHolidayHeaderRepository : IRepository<HolidayHeader>
    {
    }

    /// <summary>
    /// Access to holidays.
    /// </summary>
    public class HolidayRepository : RepositoryBase<Holiday>, IHolidayRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public HolidayRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<Holiday> GetHolidaysByHeaderId(int id)
        {
            return (from h in this.DataContext.Holidays
                    where h.HolidayHeader_Id == id
                    select h).ToList();  
        }

        public IEnumerable<Holiday> GetHolidaysByHeaderIdAndYear(int year, int id)
        {
            return (from h in this.DataContext.Holidays
                    where h.HolidayHeader_Id == id && h.HolidayDate.Year == year
                    select h).ToList();
        }

        public IList<Holiday> GetHolidaysByHeaderIdAndYearForList(int year, int id)
        {
            var query = (from h in this.DataContext.Holidays
                         where h.HolidayHeader_Id == id && h.HolidayDate.Year == year
                         select h);


            return query.OrderBy(x => x.HolidayDate).ToList();
        }
    }

    #endregion

    #region HOLIDAYHEADER

    /// <summary>
    /// The holiday header repository.
    /// </summary>
    public class HolidayHeaderRepository : RepositoryBase<HolidayHeader>, IHolidayHeaderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HolidayHeaderRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public HolidayHeaderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
