IF OBJECT_ID('vw_Case_Formfield', 'V') IS NOT NULL
BEGIN 
	DROP VIEW [dbo].[vw_Case_Formfield]
END
GO

CREATE VIEW [dbo].[vw_Case_Formfield]
AS

SELECT 
	c.[Casenumber] AS [Casenumber],
	c.[ReportedBy] AS [EmployeeNumber],
	c.[Persons_Name] AS [FirstName],
	c.[Persons_Phone] AS [LastName],
	c.[UserCode] AS [DateOfBirth],
	pa.[ProductArea] AS [SubProcess],
	ISNULL(pa1.ProductArea, pa.ProductArea) AS [MainProcess],
	r.[Region] AS [Company],
	d.[Department] AS [Unit],
	ou.[OU] AS [Department],
	ou1.[OU] AS [Function],
	ss.[StateSecondary] AS [Status],
	c.[ReferenceNumber] AS [NumberOfRecords],
	wg.[WorkingGroup] AS [WorkingGroup],
	u.[FirstName] AS [AdminName],
	u.[SurName] AS [AdminLastName],
	c.[RegUserDomain] AS [Initiator],
	[dbo].Translate(2, p.[PriorityName]) AS [SLA],
	s.[StatusName] AS [StatusName],
	c.[RegTime] AS [RegistrationTime],
	c.[FinishingDate] AS [FinishingDate],
	c.[PlanDate] AS [EffectiveDate],
	f.[FormFieldName] AS [FormFieldName],
	fv.[FormFieldValue] AS [FormFieldValue],
	fv.[Case_Id] AS [Case_Id],
	f.[Id] AS [Id],
	fv.[FormField_Id] AS [FormField_Id],
	c.[RegUserId] AS [RegUserId],
	c.[Customer_Id] AS [Customer_Id]
FROM [dbo].[tblCase] c
	JOIN [dbo].[tblFormFieldValue] fv ON c.[Id] = fv.[Case_Id]
	JOIN [dbo].[tblFormField] f ON fv.[FormField_Id] = f.[Id]
	LEFT JOIN [dbo].[tblProductArea] pa ON c.[ProductArea_Id] = pa.[Id] 
	LEFT JOIN [dbo].[tblProductArea] pa1 ON pa.[Parent_ProductArea_Id] = pa1.[Id]
	LEFT JOIN [dbo].[tblDepartment] d ON c.[Department_Id] = d.[Id]
	LEFT JOIN [dbo].[tblRegion] r ON c.[Region_Id] = r.[Id]
	LEFT JOIN [dbo].[tblStateSecondary] ss ON c.[StateSecondary_Id] = ss.[Id]
	LEFT JOIN [dbo].[tblWorkingGroup] wg ON c.[WorkingGroup_Id] = wg.Id
	LEFT JOIN [dbo].[tblUsers] u ON c.[Performer_User_Id] = u.[Id]
	LEFT JOIN [dbo].[tblPriority] p ON c.[Priority_Id] = p.[Id]
	LEFT JOIN [dbo].[tblStatus] s ON c.[Status_Id] = s.[Id]
	LEFT JOIN [dbo].[tblOU] ou ON c.[OU_Id] = ou.[Id]
	LEFT JOIN [dbo].[tblOU] ou1 ON ou.[Parent_OU_Id] = ou1.[Id]
GROUP BY c.[Casenumber],
		c.[ReportedBy],
		f.[FormFieldName],
		fv.[FormFieldValue],
		fv.[Case_Id],
		c.[RegTime],
		f.[Id],
		fv.[FormField_Id],
		pa.[ProductArea],
		pa1.[ProductArea],
		c.[FinishingDate],
		c.[Persons_Name],
		c.[Persons_Phone],
		c.[WorkingGroup_Id],
		u.[FirstName],
		u.[SurName],
		d.[Department],
		c.[RegUserDomain],
		p.[PriorityName],
		wg.[WorkingGroup],
		ss.[StateSecondary],
		r.[Region],
		c.[RegUserId],
		c.[UserCode],
		c.[ReferenceNumber],
		c.[PlanDate],
		s.[StatusName],
		ou.[OU],
		ou1.[OU],
		c.[Customer_Id]
GO

IF OBJECT_ID('ECT_Get_CaseData', 'P') IS NOT NULL
BEGIN 
	DROP PROC [dbo].[ECT_Get_CaseData]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ECT_Get_CaseData]
	@CustomerId INT
AS 

	SET NOCOUNT ON;

	DECLARE @CurDate DATETIME
	SET @CurDate = (SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())))

	SELECT *
	FROM [dbo].[vw_Case_Formfield]
	WHERE [Customer_Id] = @CustomerId
		AND ([RegistrationTime] >= @CurDate OR [FinishingDate] = @CurDate)

GO

--EXEC [dbo].[ECT_Get_CaseData] @CustomerId = 29
