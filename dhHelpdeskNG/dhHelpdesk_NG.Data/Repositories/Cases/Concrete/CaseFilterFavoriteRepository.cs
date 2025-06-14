﻿using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
 {
    

     public sealed class CaseFilterFavoriteRepository : RepositoryBase<CaseFilterFavoriteEntity>, ICaseFilterFavoriteRepository
     {         
 
         private readonly IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite> _caseFilterFavoriteToBusinessModelMapper;
         private readonly IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity> _caseFilterFavoriteToEntityMapper;

         public CaseFilterFavoriteRepository(
             IDatabaseFactory databaseFactory, 
             IEntityToBusinessModelMapper<CaseFilterFavoriteEntity, CaseFilterFavorite> caseFilterFavoriteToBusinessModelMapper, 
             IBusinessModelToEntityMapper<CaseFilterFavorite, CaseFilterFavoriteEntity> caseFilterFavoriteToEntityMapper)
             : base(databaseFactory)
         {
             this._caseFilterFavoriteToBusinessModelMapper = caseFilterFavoriteToBusinessModelMapper;
             this._caseFilterFavoriteToEntityMapper = caseFilterFavoriteToEntityMapper;
         }
 
     
         public IQueryable<CaseFilterFavoriteEntity> GetUserFavoriteFilters(int customerId, int userId)
         {
            var entities = Table.Where(f => f.Customer_Id == customerId && f.User_Id == userId);
             return entities;
         }

         public string SaveFavorite(CaseFilterFavorite favorite)
         {             
             var entities = Table.Where(f => f.Customer_Id == favorite.CustomerId && f.User_Id == favorite.UserId).ToList();

             //new mode
             if (favorite.Id == 0)
             {
                 if (!entities.Any() || !entities.Where(f => f.Name.ToLower() == favorite.Name.ToLower()).ToList().Any())
                 {
                     var entity = new CaseFilterFavoriteEntity();
                     this._caseFilterFavoriteToEntityMapper.Map(favorite, entity);
                     this.Add(entity);
                     return string.Empty;
                 }
                 else
                     return "Repeated";
             }
             else
             {
                 if (!entities.Any())
                     return "User favorites is empty!";

                 var existingFavorite = entities.Where(f => f.Id == favorite.Id && f.Name.ToLower() == favorite.Name.ToLower()).FirstOrDefault();
                 if(existingFavorite != null)
                 {                     
                     this._caseFilterFavoriteToEntityMapper.Map(favorite, existingFavorite);
                     this.Update(existingFavorite);
                     return string.Empty;
                 }
                 else
                 {
                     if (!entities.Where(f => f.Name.ToLower() == favorite.Name.ToLower()).ToList().Any())
                     {
                         var entity = new CaseFilterFavoriteEntity();
                         this._caseFilterFavoriteToEntityMapper.Map(favorite, entity);
                         this.Add(entity);
                         return string.Empty;
                     }
                     else
                        return "Repeated";
                 }
             }

         }

         public string DeleteFavorite(int favoriteId)
         {
             var entitiy = Table.Where(f => f.Id == favoriteId).FirstOrDefault();

             if (entitiy != null)
             {
                 this.Delete(entitiy);
                 return string.Empty;
             }
             else
             {
                 return "Favorite not found!";
             }             
         }
                   
     }
 }