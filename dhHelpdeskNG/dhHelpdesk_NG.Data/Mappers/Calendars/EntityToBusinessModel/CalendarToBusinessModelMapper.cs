// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarToBusinessModelMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CalendarToBusinessModelMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Calendars.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The calendar to business model mapper.
    /// </summary>
    public sealed class CalendarToBusinessModelMapper : IEntityToBusinessModelMapper<Calendar, CalendarOverview>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="CalendarOverview"/>.
        /// </returns>
        public CalendarOverview Map(Calendar entity)
        {
            if (entity == null)
            {
                return null;
            }
            
            return new CalendarOverview()
                       {
                           Id = entity.Id,
                           CustomerId = entity.Customer_Id,
                           CalendarDate = entity.CalendarDate,
                           Caption = entity.Caption,
                           Text = entity.Text,
                           ShowOnStartPage = entity.ShowOnStartPage.ToBool(),
                           ShowFromDate = entity.ShowFromDate,
                           ShowUntilDate = entity.ShowUntilDate,
                           PublicInformation = entity.PublicInformation.ToBool(),
                           ChangedByUserId = entity.ChangedByUser_Id,
                           ChangedDate = entity.ChangedDate,
                           CreatedDate = entity.CreatedDate,
                           WGs = entity.WGs
                       };
        }
    }
}