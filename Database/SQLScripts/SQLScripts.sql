--update DB from 5.3.56 to 5.3.57 version
RAISERROR ('Add Column SiteURL to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SiteURL') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblSettings
		ADD SiteURL nvarchar(100) Null
	End
Go

RAISERROR ('Add Column SelfServiceURL to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SelfServiceURL') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblSettings
		ADD SelfServiceURL nvarchar(100) Null
	End

Go

RAISERROR ('Add Column GDPRType to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','GDPRType') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD GDPRType int not Null default 1
	End

Go

RAISERROR ('Add Column CaseTypes to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','CaseTypes') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD CaseTypes nvarchar(256) Null
	End

Go

RAISERROR ('Add Column FinishedDateFrom to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','FinishedDateFrom') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD FinishedDateFrom datetime null
	End

Go

RAISERROR ('Add Column FinishedDateTo to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','FinishedDateTo') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD FinishedDateTo datetime null
	End

Go

RAISERROR ('Add Foreign Key Customer_Id to tblRegistrationSourceCustomer', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblRegistrationSourceCustomer_tblCustomer]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblRegistrationSourceCustomer] WITH CHECK ADD CONSTRAINT [FK_tblRegistrationSourceCustomer_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblMail2TicketCase', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMail2TicketCase_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblMail2TicketCase] WITH CHECK ADD CONSTRAINT [FK_tblMail2TicketCase_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblLocalAdmin', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblLocalAdmin_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblLocalAdmin] WITH CHECK ADD CONSTRAINT [FK_tblLocalAdmin_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblFormFieldValueHistory', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblFormFieldValueHistory_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblFormFieldValueHistory] WITH CHECK ADD CONSTRAINT [FK_tblFormFieldValueHistory_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Delete from tblCaseStatistics where there no longer is a connected case', 10, 1) WITH NOWAIT
DELETE cs
FROM [dbo].[tblCaseStatistics] AS cs
LEFT JOIN tblCase as c
	on cs.Case_Id = c.Id
WHERE c.Id IS NULL
GO


RAISERROR ('Add Foreign Key Case_Id to tblCaseStatistics', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseStatistics_tblCase]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblCaseStatistics] WITH CHECK ADD CONSTRAINT [FK_tblCaseStatistics_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Customer_Id to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseFilterFavorite_tblCustomer]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblCaseFilterFavorite] WITH CHECK ADD CONSTRAINT [FK_tblCaseFilterFavorite_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END
GO

RAISERROR ('Add Foreign Key Customer_Id to tblBR_Rules', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblBR_Rules_tblCustomer]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblBR_Rules] WITH CHECK ADD CONSTRAINT [FK_tblBR_Rules_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
END
GO

RAISERROR ('Delete from tblMergedCases Child where there no longer is a connected case', 10, 1) WITH NOWAIT
DELETE mcc
FROM tblMergedCases AS mcc
LEFT JOIN tblcase AS c
	ON mcc.MergedChild_Id = c.Id
WHERE c.Id IS NULL
GO

RAISERROR ('Delete from tblMergedCases Parent where there no longer is a connected case', 10, 1) WITH NOWAIT
DELETE mcp
FROM tblMergedCases AS mcp
LEFT JOIN tblcase AS c
	ON mcp.MergedParent_Id = c.Id
WHERE c.Id IS NULL
GO

RAISERROR ('Add Foreign Key Case_Id to tblMergedCases MergedParent_Id', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Parent]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblMergedCases] WITH CHECK ADD CONSTRAINT [FK_tblMergedCases_tblCase_Parent] FOREIGN KEY([MergedParent_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Foreign Key Case_Id to tblMergedCases MergedChild_Id', 10, 1) WITH NOWAIT
IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Child]') 
AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[tblMergedCases] WITH CHECK ADD CONSTRAINT [FK_tblMergedCases_tblCase_Child] FOREIGN KEY([MergedChild_Id]) REFERENCES [dbo].[tblCase] ([Id])
END
GO

RAISERROR ('Add Column ProductAreas to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','ProductAreas') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD ProductAreas nvarchar(256) Null
	End

Go

RAISERROR ('Create SQL type IdsList', 10, 1) WITH NOWAIT
IF TYPE_ID(N'IdsList') IS NULL
CREATE TYPE IdsList AS TABLE ( Id INT );
GO

RAISERROR ('Create SQL Procedure [dbo].[sp_DeleteCases]', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[sp_DeleteCases]', 'P') IS NOT NULL)
DROP PROCEDURE sp_DeleteCases
GO

CREATE PROCEDURE [dbo].[sp_DeleteCases] 
	@Cases IdsList READONLY
AS
BEGIN

	BEGIN TRAN	
		BEGIN TRY

			DELETE lu
			FROM tblLink_tblUsers AS lu
			INNER JOIN dbo.tblLink AS l
				ON lu.Link_Id = l.Id
			INNER JOIN dbo.tblCaseSolution  AS cs 
				ON l.CaseSolution_Id= cs.Id
			INNER JOIN dbo.tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id;

			DELETE lw
			FROM tblLink_tblWorkingGroup AS lw
			INNER JOIN tblLink AS l	
				ON lw.Link_Id = l.Id
			INNER JOIN dbo.tblCaseSolution AS cs
				ON l.CaseSolution_Id = cs.Id
			INNER JOIN tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;

			DELETE cl
			FROM tblChangeLog AS cl
			INNER JOIN dbo.tblChangeEMailLog AS cel
				ON cl.ChangeEMailLog_Id = cel.Id
			INNER JOIN dbo.tblChangeHistory AS chh
				ON cel.ChangeHistory_Id = chh.Id
			INNER JOIN dbo.tblChange AS ch
				on chh.Change_Id = ch.Id
			INNER JOIN @Cases AS c
				on ch.SourceCase_Id = c.Id;

			DELETE tpr 
			FROM tblParentChildCaseRelations AS tpr
			INNER JOIN @Cases AS c
				ON c.Id = tpr.Ancestor_Id;

			DELETE tec
			FROM tblCase_ExtendedCaseData AS tec
			INNER JOIN @Cases AS c
				ON c.Id = tec.Case_Id;

			DELETE tca
			FROM tblCaseIsAbout AS tca
			INNER JOIN @Cases AS c
				ON c.Id = tca.Case_Id;

			DELETE cq
			FROM tblCaseQuestion AS cq
			INNER JOIN dbo.tblCaseQuestionCategory  AS cqc
				on cq.CaseQuestionCategory_Id = cqc.Id
			INNER JOIN dbo.tblCaseQuestionHeader AS cqh
				on cqc.CaseQuestionHeader_Id = cqh.Id
			INNER JOIN @Cases AS c
				on cqh.Case_Id = c.Id;

			DELETE cqc
			FROM tblCaseQuestionCategory AS cqc
			INNER JOIN dbo.tblCaseQuestionHeader AS cqh
				on cqc.CaseQuestionHeader_Id = cqh.Id
			INNER JOIN @Cases AS c
				on cqh.Case_Id = c.Id; 
 
 			DELETE cqh
			FROM tblCaseQuestionHeader AS cqh
			INNER JOIN @Cases AS c 
				ON cqh.Case_Id = c.Id;
			
 			DELETE lfe
			FROM tblLogFileExisting AS lfe
			INNER JOIN dbo.tblLog AS l
				ON lfe.Log_Id = l.Id
			INNER JOIN @Cases AS c 
				ON l.Case_Id = c.Id;

			DELETE cd
			FROM tblChange_tblDepartment AS cd
			INNER JOIN dbo.tblChange AS ch
				ON cd.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;
 
			DELETE ccg
			FROM tblChange_tblChangeGroup AS ccg
			INNER JOIN tblChange AS ch
				ON ccg.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id; 
	 
	 		DELETE cir
			FROM tblCaseInvoiceRow AS cir
			INNER JOIN tblInvoiceRow AS ir 
				ON cir.InvoiceRow_Id = ir.Id
			INNER JOIN @Cases AS c
				ON ir.Case_Id = c.Id;
			
			DELETE cir
			FROM tblCaseInvoiceRow AS cir
			INNER JOIN @Cases AS c
				ON cir.Case_Id = c.Id;			

			SELECT ir.Id, ir.Case_Id, ir.InvoiceHeader_Id
			INTO #tmpInvoiceRow
			FROM tblInvoiceRow AS ir
			INNER JOIN @Cases AS c
				ON ir.Case_Id = c.Id;

			DELETE ir
			FROM tblInvoiceRow AS ir
			INNER JOIN @Cases AS c
				ON ir.Case_Id = c.Id;

			DELETE ih
			FROM tblInvoiceHeader AS ih
			INNER JOIN #tmpInvoiceRow AS ir
				ON ir.InvoiceHeader_Id = ih.Id
			INNER JOIN @Cases AS c
				ON ir.Case_Id = c.Id;
			
			DROP TABLE #tmpInvoiceRow;

			DELETE cc
			FROM tblChangeCouncil AS cc
			INNER JOIN tblChange AS ch
				ON cc.Change_Id = ch.Id
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id;

			DELETE cc
			FROM tblChangeContact AS cc
			INNER JOIN dbo.tblChange AS ch
				ON cc.Change_Id = ch.Id
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id; 

			DELETE cch
			FROM tblChange_tblChange AS cch
			INNER JOIN dbo.tblChange AS ch
				ON cch.RelatedChange_Id = ch.Id
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id;	

			DELETE cf
			FROM tblChangeFile AS cf
			INNER JOIN dbo.tblChange AS ch
				ON cf.Change_Id = ch.Id
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id; 

			SELECT tl.Id, tl.Case_Id 
			INTO #TmpLogs
			FROM tblLog AS tl 
			INNER JOIN @Cases AS c
				ON c.Id = tl.Case_Id;

			DELETE lf
			FROM tblLogFile AS lf
			INNER JOIN #TmpLogs AS tmp
				ON tmp.Id = lf.Log_Id;

			DELETE m2t
			FROM tblMail2Ticket AS m2t
			INNER JOIN #TmpLogs AS tmp
				ON tmp.Id = m2t.Log_Id;

			DELETE m2tc
			FROM tblMail2TicketCase AS m2tc
			INNER JOIN #TmpLogs AS tmp
				ON tmp.Id = m2tc.Case_Id;

			DELETE m2t
			FROM tblMail2Ticket AS m2t
			INNER JOIN @Cases AS c
				ON c.Id = m2t.Case_Id;

			DELETE ela
			FROM tblEmailLogAttempts AS ela
			INNER JOIN #TmpLogs AS tmp
				ON tmp.Id = ela.EmailLog_Id;

			SELECT ch.Id, ch.Case_Id
			INTO #TmpCaseHistory
			FROM tblCaseHistory AS ch
			INNER JOIN @Cases AS c
				ON ch.Case_Id = c.Id;

			DELETE ela
			FROM tblEmailLogAttempts AS ela
			INNER JOIN tblEmailLog AS el
				ON ela.EmailLog_Id = el.Id
			INNER JOIN #TmpCaseHistory AS ch
				ON ch.Id = el.CaseHistory_Id;
							   
			DELETE el
			FROM tblEmailLog AS el
			INNER JOIN #TmpCaseHistory AS ch
				ON ch.Id = el.CaseHistory_Id;
		
			DELETE tl
			FROM tblLog AS tl
			INNER JOIN #TmpLogs As l
				ON l.Id = tl.Id;

			DROP TABLE #TmpLogs;

			DELETE ch
			FROM tblCaseHistory AS ch
			INNER JOIN #TmpCaseHistory AS t
				ON t.Id = ch.Id;

			DROP TABLE #TmpCaseHistory;

			DELETE cl
			FROM tblCaseLock AS cl
			INNER JOIN  @Cases AS c
				ON c.Id = cl.Case_Id;

			DELETE cf
			FROM tblCaseFile AS cf
			INNER JOIN  @Cases AS c
				ON c.Id = cf.Case_Id;

			DELETE fvl
			FROM tblFileViewLog AS fvl
			INNER JOIN  @Cases AS c
				ON c.Id = fvl.Case_Id;

			SELECT ci.Id
			INTO #TmpCaseInvoice
			FROM tblCaseInvoice AS ci
			INNER JOIN  @Cases AS c
				ON c.Id = ci.CaseId;
			
			DELETE cia
			FROM tblCaseInvoiceArticle AS cia
			INNER JOIN tblCaseInvoiceOrder AS cio
				ON cio.Id = cia.OrderId
			INNER JOIN #TmpCaseInvoice AS t
				ON t.Id = cio.InvoiceId;

			DELETE cof
			FROM tblCaseInvoiceOrderFile AS cof
			INNER JOIN tblCaseInvoiceOrder AS cio
				ON cio.Id = cof.OrderId
			INNER JOIN #TmpCaseInvoice AS t
				ON t.Id = cio.InvoiceId;
		
			DELETE cio
			FROM tblCaseInvoiceOrder AS cio
			INNER JOIN #TmpCaseInvoice AS t
				ON t.Id = cio.InvoiceId;

			DELETE ci
			FROM tblCaseInvoice AS ci
			INNER JOIN #TmpCaseInvoice AS t
				ON t.Id = ci.Id;

			DROP TABLE #TmpCaseInvoice;
				
			DELETE csc
			FROM tblCaseSolutionSchedule AS csc
			INNER JOIN tblCaseSolution AS cs
				ON csc.CaseSolution_Id = cs.Id
			INNER JOIN dbo.tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;
	
			DELETE csf 
			FROM tblCaseSolutionFieldSettings  csf
			INNER JOIN tblCaseSolution AS cs
				ON csf.CaseSolution_Id = cs.Id
			INNER JOIN tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;
 
 			DELETE l
			FROM dbo.tblLink AS l
			INNER JOIN tblCaseSolution AS cs
				ON l.CaseSolution_Id = cs.Id
			INNER JOIN dbo.tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;

			DELETE cse
			FROM tblCaseSolution_tblCaseSection_ExtendedCaseForm AS cse
			INNER JOIN dbo.tblCaseSolution AS cs
				on cse.tblCaseSolutionID = cs.Id
			INNER JOIN dbo.tblChange AS ch
				on cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				on ch.SourceCase_Id = c.Id;

			DELETE cse
			FROM tblCaseSolution_ExtendedCaseForms AS cse
			INNER JOIN dbo.tblCaseSolution AS cs 
				ON cse.CaseSolution_Id = cs.Id
			INNER JOIN dbo.tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;

			DELETE csc
			FROM tblCaseSolutionCondition AS csc
			INNER JOIN tblCaseSolution AS cs 
				ON csc.CaseSolution_Id=cs.Id
			INNER JOIN tblChange AS ch
				ON cs.Change_Id=ch.Id
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id; 

			DELETE cscl
			FROM tblCaseSolutionConditionLog AS cscl
			INNER JOIN dbo.tblCaseSolution AS cs
				ON cscl.CaseSolution_Id = cs.Id
			INNER JOIN dbo.tblChange AS ch
				ON cs.Change_Id = ch.Id
			INNER JOIN @Cases AS c 
				ON ch.SourceCase_Id = c.Id;
  
			DELETE ch
			FROM tblChange AS ch
			INNER JOIN @Cases AS c
				ON ch.SourceCase_Id = c.Id;
 
			DELETE cl
			FROM tblContractLog AS cl
			INNER JOIN  @Cases AS c
				ON c.Id = cl.Case_Id;
		
			DELETE flu
			FROM tblCaseFollowUps AS flu
			INNER JOIN  @Cases AS c
				ON c.Id = flu.Case_Id;

			DELETE ef
			FROM tblCaseExtraFollowers AS ef
			INNER JOIN  @Cases AS c
				ON c.Id = ef.Case_Id;
	
			DELETE tecd
			FROM tblCase_tblCaseSection_ExtendedCaseData AS tecd
			INNER JOIN  @Cases AS c
				ON c.Id = tecd.Case_Id;

			SELECT cp.Id
			INTO #TmpQuestionnaireCircularPart
			FROM tblQuestionnaireCircularPart AS cp
			INNER JOIN  @Cases AS c
				ON c.Id = cp.Case_Id;

			DELETE qqr
			FROM tblQuestionnaireResult AS qr			
			INNER JOIN #TmpQuestionnaireCircularPart AS cp
				ON qr.QuestionnaireCircularPartic_Id = cp.Id
			INNER JOIN tblQuestionnaireQuestionResult AS qqr
				ON qqr.QuestionnaireResult_Id = qr.Id;

			DELETE qr
			FROM tblQuestionnaireResult AS qr			
			INNER JOIN #TmpQuestionnaireCircularPart AS cp
				ON qr.QuestionnaireCircularPartic_Id = cp.Id;

			DELETE cp
			FROM tblQuestionnaireCircularPart AS cp
			INNER JOIN #TmpQuestionnaireCircularPart AS t
				ON t.Id = cp.Id;

			DROP TABLE #TmpQuestionnaireCircularPart;

			DELETE mc
			FROM tblMergedCases AS mc
			INNER JOIN  @Cases AS c
				ON c.Id = mc.MergedChild_Id OR c.Id = mc.MergedParent_Id;

			DELETE lp 
			FROM tblLogProgram AS lp
			INNER JOIN @Cases AS c
				ON c.Id = lp.Case_Id;
			
			DELETE la 
			FROM tblLocalAdmin AS la
			INNER JOIN @Cases AS c
				ON c.Id = la.Case_Id;		
			
			DELETE vh
			FROM tblFormFieldValueHistory AS vh
			INNER JOIN @Cases AS c
				ON c.Id = vh.Case_Id;	

			delete vh
			from tblFormFieldValueHistory AS vh
			INNER JOIN tblFormFieldValue AS fv 
				ON fv.Case_Id = vh.FormField_Id
			INNER JOIN @Cases AS c 
				ON fv.Case_Id = c.Id;

			DELETE tfv
			FROM tblFormFieldValue AS tfv
			INNER JOIN @Cases AS c
				ON c.Id = tfv.Case_Id;
			
			DELETE cs
			FROM tblCaseStatistics AS cs
			INNER JOIN @Cases AS c
				ON c.Id = cs.Case_Id;			 

			DELETE tc
			FROM tblCase AS tc
			INNER JOIN  @Cases AS c
				ON tc.Id = c.Id;
	
		COMMIT TRAN;
	END TRY

	BEGIN CATCH

	 IF @@TRANCOUNT > 0
		ROLLBACK TRAN

		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
		DECLARE @ErrorState INT = ERROR_STATE()

		-- Use RAISERROR inside the CATCH block to return error  
		-- information about the original error that caused  
		-- execution to jump to the CATCH block.  
		RAISERROR (@ErrorMessage, -- Message text.  
				   @ErrorSeverity, -- Severity.  
				   @ErrorState -- State.  
				   );
	END CATCH
END	

GO

RAISERROR ('Add Column AnonymizationPermission to tblGDPRDataPrivacyAccess', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyAccess','AnonymizationPermission') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyAccess
		ADD AnonymizationPermission int not null default 1 
	End

Go

RAISERROR ('Add Column DeletionPermission to tblGDPRDataPrivacyAccess', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyAccess','DeletionPermission') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyAccess
		ADD DeletionPermission int not null default 0 
	End

Go

RAISERROR ('Add Column ErrorResult to tblGDPROperationsAudit', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPROperationsAudit','ErrorResult') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPROperationsAudit
		ADD ErrorResult NVARCHAR(MAX) NULL
	End

Go

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.57'
GO