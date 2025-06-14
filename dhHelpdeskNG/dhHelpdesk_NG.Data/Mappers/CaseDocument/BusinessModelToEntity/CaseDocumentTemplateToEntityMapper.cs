﻿namespace DH.Helpdesk.Dal.Mappers.CaseDocument
{
    using BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;

    public sealed class CaseDocumentTemplateToEntityMapper : IBusinessModelToEntityMapper<CaseDocumentTemplateModel, CaseDocumentTemplateEntity>
    {
        public void Map(CaseDocumentTemplateModel businessModel, CaseDocumentTemplateEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.Name = businessModel.Name;
            entity.CaseDocumentTemplateGUID = businessModel.CaseDocumentTemplateGUID;
            entity.MarginTop = businessModel.MarginTop;
            entity.MarginBottom = businessModel.MarginBottom;
            entity.MarginLeft = businessModel.MarginLeft;
            entity.MarginRight = businessModel.MarginRight;
            entity.FooterHeight = businessModel.FooterHeight;
            entity.HeaderHeight = businessModel.HeaderHeight;
            entity.ShowFooterFromPageNr = businessModel.ShowFooterFromPageNr;
            entity.ShowHeaderFromPageNr = businessModel.ShowHeaderFromPageNr;
            entity.Style = businessModel.Style;
            entity.ShowAlternativeHeaderOnFirstPage = businessModel.ShowAlternativeHeaderOnFirstPage;
            entity.ShowAlternativeFooterOnFirstPage = businessModel.ShowAlternativeFooterOnFirstPage;
            entity.DraftHeight = businessModel.DraftHeight;
            entity.DraftYLocation = businessModel.DraftYLocation;
            entity.DraftRotateAngle = businessModel.DraftRotateAngle;
            entity.HtmlViewerWidth = businessModel.HtmlViewerWidth;
        }
    }
}