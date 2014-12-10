using DH.Helpdesk.BusinessData.Models.Checklists.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.BusinessLogic.Mappers
{
    public static class CheckListsMapper
    {
        /*public static List<CheckListBM> MapToOverviews(List<Checklists> query)
        {
            var entities = query.Select(c => new
            {                
                c.Customer_Id,
                c.Id,
                c.WorkingGroup_Id,
                c.ChecklistName,
                c.CreatedDate,
                c.ChangedDate           
            }).ToList();

            //return entities.Select(c => new CheckListBM
            //{
            //    CustomerId = c.Customer_Id,
            //    Id = c.Id,
            //    WorkingGroupId = c.WorkingGroup_Id,
            //    ChecklistName = c.ChecklistName,
            //    CreatedDate = c.CreatedDate,
            //    ChangedDate = c.ChangedDate

            //}).ToList();
        }

        public static CheckListBM MapToOverview(Checklists entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CheckListBM
            {
                //CustomerId = entity.Customer_Id,
                //Id = entity.Id,
                //WorkingGroupId = entity.WorkingGroup_Id,
                //ChecklistName = entity.ChecklistName,                           
                //CreatedDate = entity.CreatedDate,
                //ChangedDate = entity.ChangedDate               
            };
        }

        public static void MapToEntity(CheckListBM model, Checklists entity)
        {
            entity.Customer_Id = model.CustomerId;
            entity.WorkingGroup_Id = model.WorkingGroupId;
            entity.ChecklistName = model.ChecklistName;
            entity.Id = model.Id;           
            entity.CreatedDate = model.CreatedDate;
            entity.ChangedDate = model.ChangedDate;
        }

        public static ChecklistService MapServicesToEntity(ChecklistServiceBM model)
        {
             return new ChecklistService
            {
              Customer_Id = model.CustomerId,
              CheckList_Id = model.Id,
              Name = model.Name,
              IsActive = model.IsActive,
              CreatedDate = model.CreatedDate,
              ChangedDate = model.ChangedDate
            };
        }
        */
    }

}
