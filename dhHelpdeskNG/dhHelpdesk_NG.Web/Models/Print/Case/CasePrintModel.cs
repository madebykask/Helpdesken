// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CasePrintModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CasePrintModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Models.Print.Case
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The case print model.
    /// </summary>
    public sealed class CasePrintModel
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the case.
        /// </summary>
        public CaseOverview Case { get; set; }

        /// <summary>
        /// Gets or sets the case log.
        /// </summary>
        public CaseLog CaseLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is reported by visible.
        /// </summary>
        public bool IsReportedByVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons name visible.
        /// </summary>
        public bool IsPersonsNameVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons email visible.
        /// </summary>
        public bool IsPersonsEmailVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons phone visible.
        /// </summary>
        public bool IsPersonsPhoneVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons cell phone visible.
        /// </summary>
        public bool IsPersonsCellPhoneVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is region visible.
        /// </summary>
        public bool IsRegionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is department visible.
        /// </summary>
        public bool IsDepartmentVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is field visible.
        /// </summary>
        public bool IsOuVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is place visible.
        /// </summary>
        public bool IsPlaceVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is user code visible.
        /// </summary>
        public bool IsUserCodeVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is inventory number visible.
        /// </summary>
        public bool IsInventoryNumberVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is inventory type visible.
        /// </summary>
        public bool IsInventoryTypeVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is inventory location visible.
        /// </summary>
        public bool IsInventoryLocationVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is case number visible.
        /// </summary>
        public bool IsCaseNumberVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is user visible.
        /// </summary>
        public bool IsUserVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is case type visible.
        /// </summary>
        public bool IsCaseTypeVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is system visible.
        /// </summary>
        public bool IsSystemVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is urgency visible.
        /// </summary>
        public bool IsUrgencyVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is impact visible.
        /// </summary>
        public bool IsImpactVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is category visible.
        /// </summary>
        public bool IsCategoryVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is supplier visible.
        /// </summary>
        public bool IsSupplierVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is invoice number visible.
        /// </summary>
        public bool IsInvoiceNumberVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is reference number visible.
        /// </summary>
        public bool IsReferenceNumberVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is caption visible.
        /// </summary>
        public bool IsCaptionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is description visible.
        /// </summary>
        public bool IsDescriptionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is miscellaneous visible.
        /// </summary>
        public bool IsMiscellaneousVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is product area visible.
        /// </summary>
        public bool IsProductAreaVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is contact before action visible.
        /// </summary>
        public bool IsContactBeforeActionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is field visible.
        /// </summary>
        public bool IsSmsVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is agreed date visible.
        /// </summary>
        public bool IsAgreedDateVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is available visible.
        /// </summary>
        public bool IsAvailableVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is cost visible.
        /// </summary>
        public bool IsCostVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is files visible.
        /// </summary>
        public bool IsFilesVisible { get; set; }

        /// <summary>
        /// Gets or sets the case files model.
        /// </summary>
        public FilesModel CaseFilesModel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is working group visible.
        /// </summary>
        public bool IsWorkingGroupVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is case responsible user visible.
        /// </summary>
        public bool IsCaseResponsibleUserVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is performer user visible.
        /// </summary>
        public bool IsPerformerUserVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is priority visible.
        /// </summary>
        public bool IsPriorityVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is status visible.
        /// </summary>
        public bool IsStatusVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is state secondary visible.
        /// </summary>
        public bool IsStateSecondaryVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is watch date visible.
        /// </summary>
        public bool IsWatchDateVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is verified visible.
        /// </summary>
        public bool IsVerifiedVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is verified description visible.
        /// </summary>
        public bool IsVerifiedDescriptionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is solution rate visible.
        /// </summary>
        public bool IsSolutionRateVisible { get; set; }

        /// <summary>
        /// Gets or sets the empty case history.
        /// </summary>
        public CaseHistory EmptyCaseHistory { get; set; }

        /// <summary>
        /// Gets or sets the case field settings.
        /// </summary>
        public IList<CaseFieldSetting> CaseFieldSettings { get; set; }

        /// <summary>
        /// Gets or sets the department filter format.
        /// </summary>
        public int DepartmentFilterFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is log text internal visible.
        /// </summary>
        public bool IsLogTextInternalVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is log text external visible.
        /// </summary>
        public bool IsLogTextExternalVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is finishing description visible.
        /// </summary>
        public bool IsFinishingDescriptionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is finishing date visible.
        /// </summary>
        public bool IsFinishingDateVisible { get; set; }

        /// <summary>
        /// Gets or sets the finishing causes.
        /// </summary>
        public IList<FinishingCause> FinishingCauses { get; set; }

        /// <summary>
        /// The get is computer information visible.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool GetIsComputerInformationVisible()
        {
            return this.IsInventoryNumberVisible || 
                this.IsInventoryLocationVisible || 
                this.IsInventoryTypeVisible;
        }
    }
}