// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerSettings.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CustomerSettings type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Customer
{
    using DH.Helpdesk.BusinessData.Enums.Users;

    /// <summary>
    /// The customer settings.
    /// </summary>
    public sealed class CustomerSettings
    {
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module account.
        /// </summary>
        public bool ModuleAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module ad sync.
        /// </summary>
        public bool ModuleAdSync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module asset.
        /// </summary>
        public bool ModuleAsset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module bulletin board.
        /// </summary>
        public bool ModuleBulletinBoard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module calendar.
        /// </summary>
        public bool ModuleCalendar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module case.
        /// </summary>
        public bool ModuleCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module change management.
        /// </summary>
        public bool ModuleChangeManagement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module checklist.
        /// </summary>
        public bool ModuleChecklist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module computer user.
        /// </summary>
        public bool ModuleComputerUser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module contract.
        /// </summary>
        public bool ModuleContract { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module daily report.
        /// </summary>
        public bool ModuleDailyReport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module document.
        /// </summary>
        public bool ModuleDocument { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module FAQ.
        /// </summary>
        public bool ModuleFaq { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module inventory.
        /// </summary>
        public bool ModuleInventory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module inventory import.
        /// </summary>
        public bool ModuleInventoryImport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module invoice.
        /// </summary>
        public bool ModuleInvoice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module license.
        /// </summary>
        public bool ModuleLicense { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module operation log.
        /// </summary>
        public bool ModuleOperationLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module order.
        /// </summary>
        public bool ModuleOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module planning.
        /// </summary>
        public bool ModulePlanning { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module project.
        /// </summary>
        public bool ModuleProject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module problem.
        /// </summary>
        public bool ModuleProblem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module question.
        /// </summary>
        public bool ModuleQuestion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module questionnaire.
        /// </summary>
        public bool ModuleQuestionnaire { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module time registration.
        /// </summary>
        public bool ModuleTimeRegistration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module watch.
        /// </summary>
        public bool ModuleWatch { get; set; }

        public bool ModuleCaseInvoice { get; set; }

        public bool ShowCaseOverviewInfo { get; set; }

        public bool ShowStatusPanel { get; set; }
        public bool BulletinBoardWGRestriction { get; set; }
        public bool CalendarWGRestriction { get; set; }
        public bool QuickLinkWGRestriction { get; set; }
        
        public bool CreateCaseFromOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether module Extended Case is used or not.
        /// </summary>
        public bool ModuleExtendedCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where attatchements should be shown.
        /// </summary>
        public int AttachmentPlacement { get; set; }
        public int M2TNewCaseMailTo { get; set; }
        public int DontConnectUserToWorkingGroup { get; set; }
        public string PhysicalFilePath { get; set; }
        public string VirtualFilePath { get; set; }
        public string BlockedEmailRecipients { get; set; }
        public int IsUserFirstLastNameRepresentation { get; set; }
        public int DepartmentFilterFormat { get; set; }
        public int DefaultCaseTemplateId { get; set; }
        public bool SetUserToAdministrator { get; set; }
        public int? DefaultAdministratorId { get; set; }

        public int ComputerUserSearchRestriction { get; set; }
        public int TimeZoneOffset { get; set; }
        public bool DisableCaseEndDate { get; set; }
        public bool CreateComputerFromOrder { get; set; }

        /// <summary>
        /// The is module on.
        /// </summary>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsModuleOn(Module module)
        {
            switch (module)
            {
                case Module.BulletinBoard:
                    return this.ModuleBulletinBoard;
                case Module.Calendar:
                    return this.ModuleCalendar;
                case Module.Customers:
                    return true;
                case Module.DailyReport:
                    return this.ModuleDailyReport;
                case Module.Documents:
                    return this.ModuleDocument;
                case Module.Faq:
                    return this.ModuleFaq;
                case Module.OperationalLog:
                    return this.ModuleOperationLog;
                case Module.Problems:
                    return this.ModuleProblem;
                case Module.QuickLinks:
                    return true;
                case Module.Statistics:
                    return true;
                case Module.ChangeManagement:
                    return this.ModuleChangeManagement;
                case Module.Cases:
                    return this.ModuleCase;
            }

            return true;
        }
    }
}