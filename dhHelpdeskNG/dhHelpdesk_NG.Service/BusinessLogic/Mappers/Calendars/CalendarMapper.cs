namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Calendars
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;

    public static class CalendarMapper
    {
        public static CalendarOverview[] MapToOverviews(this IQueryable<Calendar> query)
        {
            var entities = query.Select(c => new
                                        {
                                            c.Id,
                                            c.CalendarDate,
                                            c.Caption,
                                            c.Text,
                                            c.ShowOnStartPage,
                                            c.ShowUntilDate,
                                            c.PublicInformation
                                        }).ToArray();

            return entities.Select(c => new CalendarOverview
                                        {
                                            Id = c.Id,
                                            CalendarDate = c.CalendarDate,
                                            Caption = c.Caption,
                                            Text = c.Text,
                                            ShowOnStartPage = c.ShowOnStartPage.ToBool(),
                                            ShowUntilDate = c.ShowUntilDate,
                                            PublicInformation = c.PublicInformation.ToBool()
                                        }).ToArray();
        }

        public static CalendarOverview MapToOverview(Calendar entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CalendarOverview
                       {
                           Id = entity.Id,
                           CustomerId = entity.Customer_Id,
                           CalendarDate = entity.CalendarDate,
                           Caption = entity.Caption,
                           Text = entity.Text,
                           ShowOnStartPage = entity.ShowOnStartPage.ToBool(),
                           ShowUntilDate = entity.ShowUntilDate,
                           PublicInformation = entity.PublicInformation.ToBool(),
                           ChangedByUserId = entity.ChangedByUser_Id,
                           CreatedDate = entity.CreatedDate,
                           ChangedDate = entity.ChangedDate,
                           WGs = entity.WGs
                       };
        }

        public static void MapToEntity(CalendarOverview model, Calendar entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.CalendarDate = model.CalendarDate;
            entity.Caption = model.Caption;
            entity.Text = model.Text;
            entity.ShowOnStartPage = model.ShowOnStartPage.ToInt();
            entity.ShowUntilDate = model.ShowUntilDate;
            entity.PublicInformation = model.PublicInformation.ToInt();
            entity.ChangedByUser_Id = model.ChangedByUserId;
            entity.CreatedDate = model.CreatedDate;
            entity.ChangedDate = model.ChangedDate;
        }
    }
}