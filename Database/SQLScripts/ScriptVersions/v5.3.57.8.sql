--update DB from 5.3.56 to 5.3.57.8 version

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

--RAISERROR ('Add Foreign Key Customer_Id to tblRegistrationSourceCustomer', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblRegistrationSourceCustomer', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblRegistrationSourceCustomer_tblCustomer]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblRegistrationSourceCustomer] WITH CHECK ADD CONSTRAINT [FK_tblRegistrationSourceCustomer_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
--	END
--END
--GO

RAISERROR ('Delete from tblMail2TicketCase where there no longer is a connected case', 10, 1) WITH NOWAIT
IF (OBJECT_ID ('tblMail2TicketCase', 'U') IS NOT NULL)
BEGIN
	DELETE mtc
	FROM [dbo].tblMail2TicketCase AS mtc
	LEFT JOIN tblCase as c
		on mtc.Case_Id = c.Id
	WHERE c.Id IS NULL
END
GO

--RAISERROR ('Add Foreign Key Case_Id to tblMail2TicketCase', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblMail2TicketCase', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMail2TicketCase_tblCase]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblMail2TicketCase] WITH CHECK ADD CONSTRAINT [FK_tblMail2TicketCase_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
--	END
--END
--GO

RAISERROR ('Delete from tblLocalAdmin where there no longer is a connected case', 10, 1) WITH NOWAIT
IF (OBJECT_ID ('tblLocalAdmin', 'U') IS NOT NULL)
BEGIN
	DELETE la
	FROM [dbo].tblLocalAdmin AS la
	LEFT JOIN tblCase as c
		on la.Case_Id = c.Id
	WHERE c.Id IS NULL
END
GO

--RAISERROR ('Add Foreign Key Case_Id to tblLocalAdmin', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblLocalAdmin', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblLocalAdmin_tblCase]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblLocalAdmin] WITH CHECK ADD CONSTRAINT [FK_tblLocalAdmin_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
--	END
--END
--GO


RAISERROR ('Delete from tblFormFieldValueHistory where there no longer is a connected case', 10, 1) WITH NOWAIT
IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL)
BEGIN
	DELETE fvh
	FROM [dbo].[tblFormFieldValueHistory] AS fvh
	LEFT JOIN tblCase as c
		on fvh.Case_Id = c.Id
	WHERE c.Id IS NULL
END
GO

--RAISERROR ('Add Foreign Key Case_Id to tblFormFieldValueHistory', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblFormFieldValueHistory_tblCase]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblFormFieldValueHistory] WITH CHECK ADD CONSTRAINT [FK_tblFormFieldValueHistory_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
--	END
--END
--GO

RAISERROR ('Delete from tblCaseStatistics where there no longer is a connected case', 10, 1) WITH NOWAIT
IF (OBJECT_ID ('tblCaseStatistics', 'U') IS NOT NULL)
BEGIN
	DELETE cs
	FROM [dbo].[tblCaseStatistics] AS cs
	LEFT JOIN tblCase as c
		on cs.Case_Id = c.Id
	WHERE c.Id IS NULL
END
GO


--RAISERROR ('Add Foreign Key Case_Id to tblCaseStatistics', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblCaseStatistics', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseStatistics_tblCase]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblCaseStatistics] WITH CHECK ADD CONSTRAINT [FK_tblCaseStatistics_tblCase] FOREIGN KEY([Case_Id]) REFERENCES [dbo].[tblCase] ([Id])
--	END
--END
--GO

--RAISERROR ('Add Foreign Key Customer_Id to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblCaseFilterFavorite', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseFilterFavorite_tblCustomer]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblCaseFilterFavorite] WITH CHECK ADD CONSTRAINT [FK_tblCaseFilterFavorite_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
--	END
--END
--GO

--RAISERROR ('Add Foreign Key Customer_Id to tblBR_Rules', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblBR_Rules', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblBR_Rules_tblCustomer]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblBR_Rules] WITH CHECK ADD CONSTRAINT [FK_tblBR_Rules_tblCustomer] FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[tblCustomer] ([Id])
--	END
--END
--GO

RAISERROR ('Delete from tblMergedCases Child where there no longer is a connected case', 10, 1) WITH NOWAIT
IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
BEGIN
	DELETE mcc
	FROM tblMergedCases AS mcc
	LEFT JOIN tblcase AS c
		ON mcc.MergedChild_Id = c.Id
	WHERE c.Id IS NULL
END
GO

RAISERROR ('Delete from tblMergedCases Parent where there no longer is a connected case', 10, 1) WITH NOWAIT
IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
BEGIN
	DELETE mcp
	FROM tblMergedCases AS mcp
	LEFT JOIN tblcase AS c
		ON mcp.MergedParent_Id = c.Id
	WHERE c.Id IS NULL
END
GO

--RAISERROR ('Add Foreign Key Case_Id to tblMergedCases MergedParent_Id', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Parent]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblMergedCases] WITH CHECK ADD CONSTRAINT [FK_tblMergedCases_tblCase_Parent] FOREIGN KEY([MergedParent_Id]) REFERENCES [dbo].[tblCase] ([Id])
--	END
--END
--GO

--RAISERROR ('Add Foreign Key Case_Id to tblMergedCases MergedChild_Id', 10, 1) WITH NOWAIT
--IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
--BEGIN
--	IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Child]') 
--	AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
--	BEGIN
--		ALTER TABLE [dbo].[tblMergedCases] WITH CHECK ADD CONSTRAINT [FK_tblMergedCases_tblCase_Child] FOREIGN KEY([MergedChild_Id]) REFERENCES [dbo].[tblCase] ([Id])
--	END
--END
--GO

RAISERROR ('Add Column ProductAreas to tblGDPRDataPrivacyFavorite', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','ProductAreas') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPRDataPrivacyFavorite
		ADD ProductAreas nvarchar(256) Null
	End

Go

RAISERROR ('Create SQL type IdsList', 10, 1) WITH NOWAIT
IF TYPE_ID(N'dbo.IdsList') IS NULL
CREATE TYPE dbo.IdsList AS TABLE ( Id INT );
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

		IF (OBJECT_ID ('tblLink_tblUsers', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLink', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
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
			END

		IF (OBJECT_ID ('tblLink_tblWorkingGroup', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLink', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
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
			END

		IF (OBJECT_ID ('tblChangeLog', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChangeEMailLog', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblChangeHistory', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
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
			END

		IF (OBJECT_ID ('tblParentChildCaseRelations', 'U') IS NOT NULL)
			BEGIN
				DELETE tpr 
				FROM tblParentChildCaseRelations AS tpr
				INNER JOIN @Cases AS c
					ON c.Id = tpr.Descendant_Id;
			END

		IF (OBJECT_ID ('tblParentChildCaseRelations', 'U') IS NOT NULL)
			BEGIN
				DELETE tpr 
				FROM tblParentChildCaseRelations AS tpr
				INNER JOIN @Cases AS c
					ON c.Id = tpr.Ancestor_Id;
			END

		IF (OBJECT_ID ('tblCase_ExtendedCaseData', 'U') IS NOT NULL)
			BEGIN
				DELETE tec
				FROM tblCase_ExtendedCaseData AS tec
				INNER JOIN @Cases AS c
					ON c.Id = tec.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseIsAbout', 'U') IS NOT NULL)
			BEGIN
				DELETE tca
				FROM tblCaseIsAbout AS tca
				INNER JOIN @Cases AS c
					ON c.Id = tca.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseQuestion', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblCaseQuestionCategory', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblCaseQuestionHeader', 'U') IS NOT NULL)
			BEGIN
				DELETE cq
				FROM tblCaseQuestion AS cq
				INNER JOIN dbo.tblCaseQuestionCategory  AS cqc
					on cq.CaseQuestionCategory_Id = cqc.Id
				INNER JOIN dbo.tblCaseQuestionHeader AS cqh
					on cqc.CaseQuestionHeader_Id = cqh.Id
				INNER JOIN @Cases AS c
					on cqh.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseQuestionCategory', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblCaseQuestionHeader', 'U') IS NOT NULL)
			BEGIN
				DELETE cqc
				FROM tblCaseQuestionCategory AS cqc
				INNER JOIN dbo.tblCaseQuestionHeader AS cqh
					on cqc.CaseQuestionHeader_Id = cqh.Id
				INNER JOIN @Cases AS c
					on cqh.Case_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblCaseQuestionHeader', 'U') IS NOT NULL)
			BEGIN
 				DELETE cqh
				FROM tblCaseQuestionHeader AS cqh
				INNER JOIN @Cases AS c 
					ON cqh.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLogFileExisting', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN
 				DELETE lfe
				FROM tblLogFileExisting AS lfe
				INNER JOIN dbo.tblLog AS l
					ON lfe.Log_Id = l.Id
				INNER JOIN @Cases AS c 
					ON l.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLogFileExisting', 'U') IS NOT NULL)
			BEGIN
				DELETE lfe
				FROM tblLogFileExisting AS lfe
				INNER JOIN @Cases AS c 
					ON lfe.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLogFileExisting', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN
				DELETE cd
				FROM tblChange_tblDepartment AS cd
				INNER JOIN dbo.tblChange AS ch
					ON cd.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChange_tblChangeGroup', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE ccg
				FROM tblChange_tblChangeGroup AS ccg
				INNER JOIN tblChange AS ch
					ON ccg.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblCaseInvoiceRow', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN
	 			DELETE cir
				FROM tblCaseInvoiceRow AS cir
				INNER JOIN tblInvoiceRow AS ir 
					ON cir.InvoiceRow_Id = ir.Id
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseInvoiceRow', 'U') IS NOT NULL)
			BEGIN
				DELETE cir
				FROM tblCaseInvoiceRow AS cir
				INNER JOIN @Cases AS c
					ON cir.Case_Id = c.Id;			
			END

		IF (OBJECT_ID ('tblChangeCouncil', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cc
				FROM tblChangeCouncil AS cc
				INNER JOIN tblChange AS ch
					ON cc.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChangeContact', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cc
				FROM tblChangeContact AS cc
				INNER JOIN dbo.tblChange AS ch
					ON cc.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblChange_tblChange', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cch
				FROM tblChange_tblChange AS cch
				INNER JOIN dbo.tblChange AS ch
					ON cch.RelatedChange_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;	
			END

		IF (OBJECT_ID ('tblChangeFile', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cf
				FROM tblChangeFile AS cf
				INNER JOIN dbo.tblChange AS ch
					ON cf.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN
				SELECT tl.Id, tl.Case_Id, tl.InvoiceRow_Id 
				INTO #TmpLogs
				FROM tblLog AS tl 
				INNER JOIN @Cases AS c
					ON c.Id = tl.Case_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLogFile', 'U') IS NOT NULL)
			BEGIN
				DELETE lf
				FROM tblLogFile AS lf
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = lf.Log_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblMail2Ticket', 'U') IS NOT NULL)
			BEGIN
				DELETE m2t
				FROM tblMail2Ticket AS m2t
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = m2t.Log_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblMail2TicketCase', 'U') IS NOT NULL)
			BEGIN
				DELETE m2tc
				FROM tblMail2TicketCase AS m2tc
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = m2tc.Case_Id;
			END

		IF (OBJECT_ID ('tblMail2Ticket', 'U') IS NOT NULL)
			BEGIN
				DELETE m2t
				FROM tblMail2Ticket AS m2t
				INNER JOIN @Cases AS c
					ON c.Id = m2t.Case_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLogAttempts', 'U') IS NOT NULL)
			BEGIN
				DELETE ela
				FROM tblEmailLogAttempts AS ela
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = ela.EmailLog_Id;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL)
			BEGIN
				SELECT ch.Id, ch.Case_Id
				INTO #TmpCaseHistory
				FROM tblCaseHistory AS ch
				INNER JOIN @Cases AS c
					ON ch.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLogAttempts', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLog', 'U') IS NOT NULL)
			BEGIN
				DELETE ela
				FROM tblEmailLogAttempts AS ela
				INNER JOIN tblEmailLog AS el
					ON ela.EmailLog_Id = el.Id
				INNER JOIN #TmpCaseHistory AS ch
					ON ch.Id = el.CaseHistory_Id;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLog', 'U') IS NOT NULL)
			BEGIN			
				DELETE el
				FROM tblEmailLog AS el
				INNER JOIN #TmpCaseHistory AS ch
					ON ch.Id = el.CaseHistory_Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN	
				SELECT ir.Id, ir.Case_Id, ir.InvoiceHeader_Id
				INTO #tmpInvoiceRow
				FROM tblInvoiceRow AS ir
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEMailLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblInvoiceHeader', 'U') IS NOT NULL)
			BEGIN	
				DELETE el 
				FROM tblEMailLog AS el
				INNER JOIN tblLog AS tl
					ON el.Log_Id = tl.Id
				INNER JOIN tblInvoiceRow AS ir
					ON tl.InvoiceRow_Id = ir.Id
				INNER JOIN tblInvoiceHeader AS ih
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN #tmpInvoiceRow AS tir
					ON tir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON tir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblEMailLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE el
				FROM tblEmailLog AS el
				INNER JOIN #TmpLogs As l
					ON l.Id = el.Log_Id;
			END
			
		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN tblInvoiceRow AS ir
					ON ir.Id = tl.InvoiceRow_Id
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblInvoiceHeader', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN tblInvoiceRow AS ir
					ON tl.InvoiceRow_Id = ir.Id
				INNER JOIN tblInvoiceHeader AS ih
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN #tmpInvoiceRow AS tir
					ON tir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON tir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN #tmpInvoiceRow as r
					ON r.Id = tl.InvoiceRow_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN #TmpLogs As l
					ON l.Id = tl.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN	
				DELETE ir
				FROM tblInvoiceRow AS ir
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN	
				DELETE ir
				FROM tblInvoiceRow AS ir
				INNER JOIN tblInvoiceHeader AS ih
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN #tmpInvoiceRow AS tir
					ON tir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON tir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpLogs') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpLogs;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblInvoiceHeader', 'U') IS NOT NULL)
			BEGIN	
				DELETE ih
				FROM tblInvoiceHeader AS ih
				INNER JOIN #tmpInvoiceRow AS ir
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tempdb..#tmpInvoiceRow') IS NOT NULL)
			BEGIN			
				DROP TABLE #tmpInvoiceRow;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL)
			BEGIN	
				DELETE ch
				FROM tblCaseHistory AS ch
				INNER JOIN #TmpCaseHistory AS t
					ON t.Id = ch.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpCaseHistory') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpCaseHistory;
			END

		IF (OBJECT_ID ('tblCaseLock', 'U') IS NOT NULL)
			BEGIN	
				DELETE cl
				FROM tblCaseLock AS cl
				INNER JOIN  @Cases AS c
					ON c.Id = cl.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseFile', 'U') IS NOT NULL)
			BEGIN
				DELETE cf
				FROM tblCaseFile AS cf
				INNER JOIN  @Cases AS c
					ON c.Id = cf.Case_Id;
			END

		IF (OBJECT_ID ('tblFileViewLog', 'U') IS NOT NULL)
			BEGIN
				DELETE fvl
				FROM tblFileViewLog AS fvl
				INNER JOIN  @Cases AS c
					ON c.Id = fvl.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				SELECT ci.Id
				INTO #TmpCaseInvoice
				FROM tblCaseInvoice AS ci
				INNER JOIN  @Cases AS c
					ON c.Id = ci.CaseId;
			END

		IF (OBJECT_ID ('tblCaseInvoiceArticle', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoiceOrder', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE cia
				FROM tblCaseInvoiceArticle AS cia
				INNER JOIN tblCaseInvoiceOrder AS cio
					ON cio.Id = cia.OrderId
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = cio.InvoiceId;
			END

		IF (OBJECT_ID ('tblCaseInvoiceOrderFile', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoiceOrder', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE cof
				FROM tblCaseInvoiceOrderFile AS cof
				INNER JOIN tblCaseInvoiceOrder AS cio
					ON cio.Id = cof.OrderId
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = cio.InvoiceId;
			END

		IF (OBJECT_ID ('tblCaseInvoiceOrder', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE cio
				FROM tblCaseInvoiceOrder AS cio
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = cio.InvoiceId;
			END

		IF (OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE ci
				FROM tblCaseInvoice AS ci
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = ci.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpCaseInvoice') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpCaseInvoice;
			END

		IF (OBJECT_ID ('tblCaseSolutionSchedule', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE csc
				FROM tblCaseSolutionSchedule AS csc
				INNER JOIN tblCaseSolution AS cs
					ON csc.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolutionFieldSettings', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE csf 
				FROM tblCaseSolutionFieldSettings  csf
				INNER JOIN tblCaseSolution AS cs
					ON csf.CaseSolution_Id = cs.Id
				INNER JOIN tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLink', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
 				DELETE l
				FROM dbo.tblLink AS l
				INNER JOIN tblCaseSolution AS cs
					ON l.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolution_tblCaseSection_ExtendedCaseForm', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cse
				FROM tblCaseSolution_tblCaseSection_ExtendedCaseForm AS cse
				INNER JOIN dbo.tblCaseSolution AS cs
					on cse.tblCaseSolutionID = cs.Id
				INNER JOIN dbo.tblChange AS ch
					on cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					on ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolution_ExtendedCaseForms', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cse
				FROM tblCaseSolution_ExtendedCaseForms AS cse
				INNER JOIN dbo.tblCaseSolution AS cs 
					ON cse.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolutionCondition', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE csc
				FROM tblCaseSolutionCondition AS csc
				INNER JOIN tblCaseSolution AS cs 
					ON csc.CaseSolution_Id=cs.Id
				INNER JOIN tblChange AS ch
					ON cs.Change_Id=ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblCaseSolutionConditionLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cscl
				FROM tblCaseSolutionConditionLog AS cscl
				INNER JOIN dbo.tblCaseSolution AS cs
					ON cscl.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE ch
				FROM tblChange AS ch
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblContractLog', 'U') IS NOT NULL)
			BEGIN
				DELETE cl
				FROM tblContractLog AS cl
				INNER JOIN  @Cases AS c
					ON c.Id = cl.Case_Id;
			END	

		IF (OBJECT_ID ('tblCaseFollowUps', 'U') IS NOT NULL)
			BEGIN
				DELETE flu
				FROM tblCaseFollowUps AS flu
				INNER JOIN  @Cases AS c
					ON c.Id = flu.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseExtraFollowers', 'U') IS NOT NULL)
			BEGIN
				DELETE ef
				FROM tblCaseExtraFollowers AS ef
				INNER JOIN  @Cases AS c
					ON c.Id = ef.Case_Id;
			END

		IF (OBJECT_ID ('tblCase_tblCaseSection_ExtendedCaseData', 'U') IS NOT NULL)
			BEGIN
				DELETE tecd
				FROM tblCase_tblCaseSection_ExtendedCaseData AS tecd
				INNER JOIN  @Cases AS c
					ON c.Id = tecd.Case_Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL)
			BEGIN
				SELECT cp.Id
				INTO #TmpQuestionnaireCircularPart
				FROM tblQuestionnaireCircularPart AS cp
				INNER JOIN  @Cases AS c
					ON c.Id = cp.Case_Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblQuestionnaireResult', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblQuestionnaireQuestionResult', 'U') IS NOT NULL)
			BEGIN
				DELETE qqr
				FROM tblQuestionnaireResult AS qr			
				INNER JOIN #TmpQuestionnaireCircularPart AS cp
					ON qr.QuestionnaireCircularPartic_Id = cp.Id
				INNER JOIN tblQuestionnaireQuestionResult AS qqr
					ON qqr.QuestionnaireResult_Id = qr.Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblQuestionnaireResult', 'U') IS NOT NULL)
			BEGIN
				DELETE qr
				FROM tblQuestionnaireResult AS qr			
				INNER JOIN #TmpQuestionnaireCircularPart AS cp
					ON qr.QuestionnaireCircularPartic_Id = cp.Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL)
			BEGIN
				DELETE cp
				FROM tblQuestionnaireCircularPart AS cp
				INNER JOIN #TmpQuestionnaireCircularPart AS t
					ON t.Id = cp.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpQuestionnaireCircularPart') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpQuestionnaireCircularPart;
			END

		IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
			BEGIN
				DELETE mc
				FROM tblMergedCases AS mc
				INNER JOIN  @Cases AS c
					ON c.Id = mc.MergedChild_Id OR c.Id = mc.MergedParent_Id;
			END

		IF (OBJECT_ID ('tblLogProgram', 'U') IS NOT NULL)
			BEGIN
				DELETE lp 
				FROM tblLogProgram AS lp
				INNER JOIN @Cases AS c
					ON c.Id = lp.Case_Id;
			END

		IF (OBJECT_ID ('tblLocalAdmin', 'U') IS NOT NULL)
			BEGIN
				DELETE la 
				FROM tblLocalAdmin AS la
				INNER JOIN @Cases AS c
					ON c.Id = la.Case_Id;		
			END

		IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL)
			BEGIN
				DELETE vh
				FROM tblFormFieldValueHistory AS vh
				INNER JOIN @Cases AS c
					ON c.Id = vh.Case_Id;	
			END

		IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL AND
			OBJECT_ID ('tblFormFieldValue', 'U') IS NOT NULL)
			BEGIN
				DELETE vh
				FROM tblFormFieldValueHistory AS vh
				INNER JOIN tblFormFieldValue AS fv 
					ON fv.Case_Id = vh.FormField_Id
				INNER JOIN @Cases AS c 
					ON fv.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblFormFieldValue', 'U') IS NOT NULL)
			BEGIN
				DELETE tfv
				FROM tblFormFieldValue AS tfv
				INNER JOIN @Cases AS c
					ON c.Id = tfv.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseStatistics', 'U') IS NOT NULL)
			BEGIN
				DELETE cs
				FROM tblCaseStatistics AS cs
				INNER JOIN @Cases AS c
					ON c.Id = cs.Case_Id;			 
			END

		IF (OBJECT_ID ('tblChecklistRow', 'U') IS NOT NULL)
			BEGIN
				DELETE cr
				FROM tblChecklistRow as cr
				INNER JOIN @Cases AS c
					ON c.Id = cr.Case_Id;
			END

		IF (OBJECT_ID ('tblCase', 'U') IS NOT NULL)
			BEGIN
				DELETE tc
				FROM tblCase AS tc
				INNER JOIN  @Cases AS c
					ON tc.Id = c.Id;
			END
		COMMIT TRAN;
	END TRY

	BEGIN CATCH

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

RAISERROR ('Add Column ErrorResultCaseNumbers to tblGDPROperationsAudit', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPROperationsAudit','ErrorResultCaseNumbers') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPROperationsAudit
		ADD ErrorResultCaseNumbers NVARCHAR(MAX) NULL
	End

Go

RAISERROR ('Add Column ResultCaseNumbers to tblGDPROperationsAudit', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPROperationsAudit','ResultCaseNumbers') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblGDPROperationsAudit
		ADD ResultCaseNumbers NVARCHAR(MAX) NULL
	End

Go

RAISERROR ('Add Column CommunicateWithPerformer to tblCustomer', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCustomer','CommunicateWithPerformer') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].tblCustomer
		ADD CommunicateWithPerformer int not null default 1
	End

Go


--HF 3
RAISERROR ('DROP Foreign Key Case_Id to tblMail2TicketCase', 10, 1) WITH NOWAIT
	IF (OBJECT_ID ('tblMail2TicketCase', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMail2TicketCase_tblCase]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblMail2TicketCase] DROP CONSTRAINT [FK_tblMail2TicketCase_tblCase]
			END
		END
		GO

RAISERROR ('DROP Foreign Key Case_Id to tblLocalAdmin', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblLocalAdmin', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblLocalAdmin_tblCase]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblLocalAdmin] DROP CONSTRAINT [FK_tblLocalAdmin_tblCase] 
			END
		END
		GO

RAISERROR ('DROP Foreign Key Case_Id to tblFormFieldValueHistory', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblFormFieldValueHistory_tblCase]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblFormFieldValueHistory] DROP CONSTRAINT [FK_tblFormFieldValueHistory_tblCase]
			END
		END
		GO

RAISERROR ('DROP Foreign Key Case_Id to tblCaseStatistics', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblCaseStatistics', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseStatistics_tblCase]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblCaseStatistics] DROP CONSTRAINT [FK_tblCaseStatistics_tblCase]
			END
		END
		GO

RAISERROR ('DROP Foreign Key Parent_Id to tblMergedCases', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Parent]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblMergedCases]  DROP CONSTRAINT [FK_tblMergedCases_tblCase_Parent] 
			END
		END
		GO

RAISERROR ('DROP Foreign Key Child_Id to tblMergedCases', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblMergedCases_tblCase_Child]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblMergedCases] DROP CONSTRAINT [FK_tblMergedCases_tblCase_Child]
			END
		END
		GO
		   


RAISERROR ('DROP Foreign Key Customer_Id to tblRegistrationSourceCustomer', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblRegistrationSourceCustomer', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblRegistrationSourceCustomer_tblCustomer]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].[tblRegistrationSourceCustomer] DROP CONSTRAINT [FK_tblRegistrationSourceCustomer_tblCustomer]
			END
		END
		GO

RAISERROR ('DROP Foreign Key Customer_Id to tblCaseFilterFavorite', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblCaseFilterFavorite', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblCaseFilterFavorite_tblCustomer]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].tblCaseFilterFavorite DROP CONSTRAINT [FK_tblCaseFilterFavorite_tblCustomer]
			END
		END
		GO

RAISERROR ('DROP Foreign Key Customer_Id to tblBR_Rules', 10, 1) WITH NOWAIT
		IF (OBJECT_ID ('tblBR_Rules', 'U') IS NOT NULL)
		BEGIN
			IF EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_tblBR_Rules_tblCustomer]') 
			AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
			BEGIN
				ALTER TABLE [dbo].tblBR_Rules DROP CONSTRAINT [FK_tblBR_Rules_tblCustomer]
			END
		END
		GO

RAISERROR ('CREATE PROCEDURE sp_GetLogFilesByCaseIds', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[sp_GetLogFilesByCaseIds]', 'P') IS NOT NULL)
BEGIN
	DROP PROCEDURE [dbo].[sp_GetLogFilesByCaseIds];
END
GO

CREATE PROCEDURE [dbo].[sp_GetLogFilesByCaseIds]
(
	@CaseIds IdsList READONLY
)

AS
BEGIN
	;WITH CTE AS (
		SELECT l.Text_External, l.Text_Internal, lf.Log_Id, lf.[FileName], 
				lf.CreatedDate, lf.ParentLog_Id, lf.IsCaseFile, lf.LogType, lf.ParentLogType
		FROM tblLog as l
		INNER JOIN @CaseIds as c
			on c.Id = l.Case_Id
		INNER JOIN tblLogFile as lf
			ON l.Id = lf.Log_Id
	)
	SELECT Log_Id, [FileName], CreatedDate, ParentLog_Id, IsCaseFile, LogType, ParentLogType
	FROM CTE AS l
END
GO

RAISERROR ('CREATE PROCEDURE sp_GetCaseFilesByCaseIds', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[sp_GetCaseFilesByCaseIds]', 'P') IS NOT NULL)
BEGIN
	DROP PROCEDURE [dbo].[sp_GetCaseFilesByCaseIds]
END
GO
CREATE PROCEDURE [dbo].[sp_GetCaseFilesByCaseIds]
(
	@CaseIds IdsList READONLY
)

AS
BEGIN

	SELECT cf.Id, cf.Case_Id, cf.[FileName], cf.CreatedDate, cf.UserId
	FROM tblCaseFile as cf
	INNER JOIN @CaseIds as c
		on c.Id = cf.Case_Id;
END
GO


-- END HF 3

  -- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.57.8'
GO