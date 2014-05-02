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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region HOLIDAY

    /// <summary>
    /// Interface for access to holidays.
    /// </summary>
    public interface IHolidayRepository : IRepository<Holiday>
    {
        /// <summary>
        /// The get holidays.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<HolidayOverview> GetHolidays();
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

        /// <summary>
        /// The get holidays.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<HolidayOverview> GetHolidays()
        {
            return this.GetAll()
                .ToList()
                .Select(h => new HolidayOverview
                                 {
                                     HolidayDate = h.HolidayDate,
                                     TimeFrom = h.TimeFrom,
                                     TimeUntil = h.TimeUntil,
                                     HolidayHeader = new HolidayHeaderOverview
                                                         {
                                                             Name = h.HolidayHeader.Name
                                                         }
                                 })
                .OrderBy(h => h.HolidayDate);
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
