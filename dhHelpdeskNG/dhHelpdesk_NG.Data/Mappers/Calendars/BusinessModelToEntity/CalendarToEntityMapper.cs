// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarToEntityMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CalendarToEntityMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Calendars.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The calendar to entity mapper.
    /// </summary>
    public sealed class CalendarToEntityMapper : IBusinessModelToEntityMapper<CalendarOverview, Calendar>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="businessModel">
        /// The business model.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Map(CalendarOverview businessModel, Calendar entity)
        {
            entity.Id = businessModel.Id;
            entity.Customer_Id = businessModel.CustomerId;
            entity.CalendarDate = businessModel.CalendarDate;
            entity.Caption = businessModel.Caption;
            entity.Text = businessModel.Text;
            entity.ShowOnStartPage = businessModel.ShowOnStartPage.ToInt();
            entity.ShowFromDate = businessModel.ShowFromDate;
            entity.ShowUntilDate = businessModel.ShowUntilDate;
            entity.PublicInformation = businessModel.PublicInformation.ToInt();
            entity.ChangedByUser_Id = businessModel.ChangedByUserId;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.WGs = businessModel.WGs;
        }
    }
}