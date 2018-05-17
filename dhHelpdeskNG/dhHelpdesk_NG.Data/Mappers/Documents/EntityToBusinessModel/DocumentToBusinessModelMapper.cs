// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentToBusinessModelMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DocumentToBusinessModelMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Documents.EntityToBusinessModel
{
    using System;
    using DH.Helpdesk.BusinessData.Models.Document.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The document to business model mapper.
    /// </summary>
    public sealed class DocumentToBusinessModelMapper : IEntityToBusinessModelMapper<Document, DocumentOverview>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentOverview"/>.
        /// </returns>
        public DocumentOverview Map(Document entity)
        {
            if (entity == null)
            {
                return null;
            }
            
            return new DocumentOverview
                       {
                           CreatedDate = entity.CreatedDate,
                           ChangedDate = entity.ChangedDate,
                           CustomerId = entity.Customer_Id,
                           Description = entity.Description,
                           Id = entity.Id,
                           Name = entity.Name,
                           Size = entity.Size,
                           ShowOnStartPage = entity.ShowOnStartPage
                       };
        }
    }
}