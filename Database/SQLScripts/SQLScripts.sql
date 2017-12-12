--update DB from 5.3.34 to 5.3.35 version

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'SplitToCaseSolutionType' and sysobjects.name = N'tblCaseSolution')
begin

	ALTER TABLE [tblCaseSolution] ADD [SplitToCaseSolutionType] int NOT NULL DEFAULT(0)

end
   


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblCaseSolution_SplitToCaseSolution' AND xtype='U')
BEGIN

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCaseSolution_SplitToCaseSolution](
		[CaseSolution_Id] [int] NOT NULL,
		[SplitToCaseSolution_Id] [int] NOT NULL,
	 CONSTRAINT [PK_tblCaseSolutionAncestor_tblCaseSolutionDescendant] PRIMARY KEY CLUSTERED 
	(
		[CaseSolution_Id] ASC,
		[SplitToCaseSolution_Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'ExtendedCaseForm_Id' and sysobjects.name = N'tblCase_ExtendedCaseData')
begin

	ALTER TABLE [tblCase_ExtendedCaseData] ADD [ExtendedCaseForm_Id] int 
end


IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceTime' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceTime] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceOvertime' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceOvertime] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoiceMaterial' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoiceMaterial] bit NOT NULL Default(1)
end

IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoicePrice' and sysobjects.name = N'tblDepartment')
begin
	ALTER TABLE [dbo].[tblDepartment] ADD [ShowInvoicePrice] bit NOT NULL Default(1)
end

if  exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'SplitToCaseSolutionType' and sysobjects.name = N'tblCaseSolution')
begin

	if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'CaseRelationType' and sysobjects.name = N'tblCaseSolution')
	begin

		EXEC sp_rename 'tblCaseSolution.SplitToCaseSolutionType', 'CaseRelationType', 'COLUMN'
	end
end

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='tblReportScheduler' AND xtype='U')
BEGIN
	CREATE TABLE [dbo].[tblReportScheduler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TargetFolder] [nvarchar](200) NOT NULL,
	[OutputFilename] [nvarchar](200) NOT NULL,
	[AppendTime] bit NOT NULL,
	[StartTime] [nvarchar](50) NOT NULL,
	[CronExpression] [nvarchar](200) NULL,
	[TimeZone] [nvarchar](200) NULL,
	[SqlQuery] [nvarchar](max) NOT NULL,
	[ExportFormat] [nvarchar](50) NOT NULL,
	[LastRun] [datetime] NULL,
 CONSTRAINT [PK_tblReportScheduler] PRIMARY KEY CLUSTERED (	[Id] ASC)
 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]

END
GO

/*Update SP Get case info */
alter Proc [dbo].[sp_GetCaseInfo] 
	@CaseId int, 	
	@LanguageId int,
	@UserId int,
	@UserTimeOffset int = 0

as
		
	Declare @CurrentCustomerId int
	Select @CurrentCustomerId = Customer_Id from tblCase where id = @CaseId

	-- *******  Temp table *********
	Declare @CaseTable Table 
		(  Id int, AgreedDate datetime, Available Nvarchar(Max), Caption Nvarchar(Max), CaseNumber numeric(18,0),
		CaseResponsibleUser_Id Nvarchar(Max), CaseType_Id Nvarchar(Max), Category_Id Nvarchar(Max),
		CausingPart Nvarchar(Max), ChangeTime datetime, ClosingReason Nvarchar(Max), ComputerType_Id Nvarchar(Max),
		ContactBeforeAction Nvarchar(Max), Cost Nvarchar(Max), CostCentre Nvarchar(Max), Customer_Id Nvarchar(Max), Department_Id Nvarchar(Max),
		Description ntext, FinishingCause_Id Nvarchar(Max), FinishingDate datetime, FinishingDescription Nvarchar(Max),
		Impact_Id Nvarchar(Max), InventoryLocation Nvarchar(Max), InventoryNumber Nvarchar(Max), InvoiceNumber Nvarchar(Max),
		IsAbout_CostCentre Nvarchar(Max), IsAbout_Department_Id Nvarchar(Max), IsAbout_OU_Id Nvarchar(Max),
		IsAbout_Persons_CellPhone Nvarchar(Max), IsAbout_Persons_EMail Nvarchar(Max), IsAbout_Persons_Name Nvarchar(Max),
		IsAbout_Persons_Phone Nvarchar(Max), IsAbout_Place Nvarchar(Max), IsAbout_Region_Id Nvarchar(Max),
		IsAbout_ReportedBy Nvarchar(Max), IsAbout_UserCode Nvarchar(Max), Miscellaneous Nvarchar(Max), OU_Id Nvarchar(Max),
		Performer_User_Id Nvarchar(Max), Persons_CellPhone Nvarchar(Max), Persons_EMail Nvarchar(Max), Persons_Name Nvarchar(Max),
		Persons_Phone Nvarchar(Max), Place Nvarchar(Max), PlanDate datetime, Priority_Id Nvarchar(Max), ProductArea_Id Nvarchar(Max),
		ReferenceNumber Nvarchar(Max), Region_Id Nvarchar(Max), RegistrationSourceCustomer Nvarchar(Max), RegTime datetime,
		ReportedBy Nvarchar(Max), SMS Nvarchar(Max), SolutionRate Nvarchar(Max), StateSecondary_Id Nvarchar(Max), 
		Status_Id Nvarchar(Max), Supplier_Id Nvarchar(Max), System_Id Nvarchar(Max), Urgency_Id Nvarchar(Max),
		User_Id Nvarchar(Max), UserCode Nvarchar(Max), WatchDate datetime, Verified Nvarchar(Max),
		VerifiedDescription Nvarchar(Max), WorkingGroup_Id Nvarchar(Max) ,
		RegUserId Nvarchar(Max), RegUserName Nvarchar(Max), RegUserDomain Nvarchar(Max),
		Project Nvarchar(Max), Problem Nvarchar(Max), Change Nvarchar(Max), CurrentUser Nvarchar(Max)
		) 

	Insert into @CaseTable 
	( Id, AgreedDate, Available, Caption , CaseNumber,
	CaseResponsibleUser_Id, CaseType_Id, Category_Id,
	CausingPart, ChangeTime, ClosingReason, ComputerType_Id,
	ContactBeforeAction, Cost, CostCentre, Customer_Id, Department_Id,
	Description, FinishingCause_Id, FinishingDate, FinishingDescription,
	Impact_Id, InventoryLocation, InventoryNumber, InvoiceNumber,
	IsAbout_CostCentre, IsAbout_Department_Id, IsAbout_OU_Id,
	IsAbout_Persons_CellPhone, IsAbout_Persons_EMail, IsAbout_Persons_Name,
	IsAbout_Persons_Phone, IsAbout_Place, IsAbout_Region_Id,
	IsAbout_ReportedBy, IsAbout_UserCode, Miscellaneous, OU_Id,
	Performer_User_Id, Persons_CellPhone, Persons_EMail, Persons_Name,
	Persons_Phone, Place, PlanDate, Priority_Id, ProductArea_Id,
	ReferenceNumber, Region_Id, RegistrationSourceCustomer, RegTime,
	ReportedBy, SMS, SolutionRate, StateSecondary_Id , 
	Status_Id, Supplier_Id, System_Id, Urgency_Id,
	User_Id, UserCode, WatchDate, Verified ,
	VerifiedDescription, WorkingGroup_Id,
	RegUserId, RegUserName, RegUserDomain, Project, Problem, Change, CurrentUser
	) 
	select Top 1
		c.Id, c.AgreedDate, c.Available, c.Caption, c.Casenumber,
		Case when se.IsUserFirstLastNameRepresentation = 1 then
			ru.FirstName + ' ' + ru.SurName
		else
			ru.SurName + ' ' + ru.FirstName 
		End as ResponsibleUserName, dbo.GetHierarchy(c.CaseType_Id, 'tblCaseType') As FullCaseTypeName,  cg.Category,

		dbo.GetHierarchy(c.CausingPartId, 'tblCausingPart') As FullCausingPartName,  c.ChangeTime,					
		dbo.GetHierarchy(LastLog.FinishingCause_Id, 'tblFinishingCause') As FullFinishingCauseName, 
		c.InventoryType, 
		case when c.ContactBeforeAction = 1 then 
				dbo.TextTranslate('Ja', @LanguageId)
			else
				dbo.TextTranslate('Nej', @LanguageId)
		end as ContactBeforeAction,
		cast(c.Cost as nvarchar(Max)) + ' - ' + cast(c.othercost as nvarchar(max)) + '  ' + c.Currency As Cost, c.CostCentre, cu.Name as CustomerName, d.Department,
		c.[Description], dbo.GetHierarchy(LastLog.FinishingCause_Id, 'tblFinishingCause') As FullFinishingCauseName, c.FinishingDate, 
		c.FinishingDescription, i.Impact, c.InventoryLocation, c.InventoryNumber, c.InvoiceNumber,
		ci.CostCentre As IsAbout_CostCentre, di.Department As IsAbout_Department, dbo.GetHierarchy(ci.OU_Id, 'tblOU') As IsAbout_FullOUName,
		ci.Person_CellPhone As IsAbout_Person_CellPhone, ci.Person_Email As IsAbout_Person_Email, ci.Person_Name As IsAbout_Person_Name,
		ci.Person_Phone As IsAbout_Person_Phone, ci.Place As IsAbout_Place, ri.Region As IsAbout_Region, 
		ci.ReportedBy As IsAbout_ReportedBy, ci.UserCode As IsAbout_UserCode, c.Miscellaneous, dbo.GetHierarchy(c.OU_Id, 'tblOU') As FullOUName, 
		Case when se.IsUserFirstLastNameRepresentation = 1 then
			pu.FirstName + ' ' + pu.SurName 
		else
			pu.SurName  + ' ' + pu.FirstName 
		End as AdminName, c.Persons_CellPhone, c.Persons_EMail, c.Persons_Name, 
		c.Persons_Phone, c.Place, c.PlanDate, pr.PriorityName, dbo.GetHierarchy(c.ProductArea_Id, 'tblProductArea') As FullProductAreaName, 
		c.ReferenceNumber, r.Region, rs.SourceName RegistrationSourceCustomer, c.RegTime, 
		c.ReportedBy, 
		case when  c.SMS = 1 then 
				dbo.TextTranslate('Ja', @LanguageId)
			else
				dbo.TextTranslate('Nej', @LanguageId)
		end as SMS,
			 
			c.SolutionRate, sub.StateSecondary,
		st.StatusName, 
		Case when sp.Country_Id is null 
				then sp.Supplier
				else ctr.Country + ' - ' + sp.Supplier end as Supplier_Id,
		s.SystemName, ur.Urgency,

		Case when se.IsUserFirstLastNameRepresentation = 1 then
			u.FirstName + ' ' + u.SurName + ' ('+ c.IPAddress + ')'  
		else
			u.SurName   + ' ' + u.FirstName + ' ('+ c.IPAddress + ')' 
		End as RegisteredBy ,  c.UserCode, c.WatchDate,
		case when  c.Verified = 1 then 
				dbo.TextTranslate('Ja', @LanguageId)
			else
				dbo.TextTranslate('Nej', @LanguageId)
		end as Verified,						
		c.VerifiedDescription, w.WorkingGroup,			 			
		c.RegUserId, c.RegUserName, c.RegUserDomain,
		prj.Name as Project, prob.ProblemName as Problem,
		ch.ChangeTitle as Change, 
		cUser.FirstName + ' ' + cUser.SurName as CurrentUser
								
		from tblCase c 
			Inner Join tblCustomer cu on (c.Customer_Id = cu.Id)	 
			Inner Join tblSettings se on (c.Customer_Id = se.Customer_Id)	 
			Left outer join tblUsers u on (c.[User_Id] = u.Id)
			Left outer join tblRegion r on (c.Region_Id = r.Id)
			Left outer join tblDepartment d on (c.Department_Id = d.Id)	 
			Left outer join tblSystem s on (c.System_Id = s.Id) 
			Left outer join tblUrgency ur on (c.Urgency_Id = ur.Id) 
			Left outer join tblImpact i on (c.Impact_Id = i.Id) 
			Left outer join tblCategory cg on (c.Category_Id = cg.Id) 
			Left outer join tblSupplier sp on (c.Supplier_Id = sp.Id) 
			Left outer join tblCountry ctr on (sp.Country_Id = ctr.Id)
			Left outer join tblUsers pu on (c.Performer_User_id = pu.Id )
			Left outer join tblUsers ru on (c.CaseResponsibleUser_Id = ru.Id )
			Left outer join tblPriority pr on (c.Priority_Id = pr.Id )
			Left outer join tblStatus st on (c.Status_Id = st.Id )
			Left outer join tblStateSecondary sub on (c.StateSecondary_Id = sub.Id)			 
			Left outer join tblWorkingGroup w on (c.WorkingGroup_Id = w.Id)
			Left outer join tblProblem prob on (c.Problem_Id = prob.Id)			 
			Left outer join tblRegistrationSourceCustomer rs on (c.RegistrationSourceCustomer_Id = rs.Id)
			Left outer join tblCaseIsAbout ci on (c.Id = ci.Case_Id)
			Left outer join tblRegion ri on (ci.Region_Id = ri.Id)
			Left outer join tblDepartment di on (ci.Department_Id = di.Id)
			Left outer join tblProject prj on (c.Project_Id = prj.Id)
			Left outer join tblChange ch on (c.Change_Id = ch.Id)
			Left outer join tblUsers cUser on (cUser.Id = @UserId)
			 
			Outer apply (select top 1 l.FinishingDate, l.FinishingType As FinishingCause_Id
						from tblLog l Left Outer Join tblFinishingCause ft on (l.FinishingType = ft.Id)
						where l.Case_Id = c.Id order by l.id desc) as LastLog

	where c.Id = @CaseId
	
	-- *********** Variables **************
	Declare @ResultSet Table (Id int, FieldName Nvarchar(Max), FieldCaption Nvarchar(Max), FieldValue Nvarchar(Max), InOrder int, LineType Nvarchar(2))    
	Declare @AvailableFields Table (FieldName Nvarchar(Max), FieldCaption Nvarchar(Max))
  
	Declare @FieldId int;
	Declare @FieldName Nvarchar(200);
	Declare @FieldCaption Nvarchar(200);
	Declare @InOrder int;

	Declare @HeaderId int;
	Declare @HeaderInOrder int;

  
	Insert into @AvailableFields 
	select cfs.CaseField, cfsl.Label   from tblCaseFieldSettings cfs
			Left Outer Join tblCaseFieldSettings_tblLang cfsl on (cfsl.CaseFieldSettings_Id = cfs.Id and cfsl.Language_Id = @LanguageId)
	Where Show = 1 and Customer_Id = @CurrentCustomerId	


	Set @FieldId = 1;
	Set @InOrder = 1;
	set @FieldName = '';
		
	Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
		Select @FieldId, @FieldName, '', c.CurrentUser, @InOrder, 'U'		
		From @CaseTable c;

	Set @FieldId = @FieldId + 1;
	Set @InOrder = @InOrder + 1;
	Set @FieldName = '';	


	Begin /* Case Field Data */
		Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'CaseNumber'  
		If (@FieldName is not null and @FieldName <> '')
		Begin 
		set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
			Select @FieldId, @FieldName, @FieldCaption, Cast(c.CaseNumber as Nvarchar(Max)) + ' - ' + cast(c.Customer_Id as Nvarchar(Max)), @InOrder, 'H'		
			From @CaseTable c;

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;
		Set @FieldName = '';
		End;  

		set @HeaderId = @FieldId;
		set @HeaderInOrder = @InOrder;

		Begin /* User information  */	     			  

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ReportedBy'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.ReportedBy as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Persons_Name'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Persons_Name as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Persons_EMail'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Persons_EMail as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;
		  
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Persons_Phone'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Persons_Phone as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Persons_CellPhone'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Persons_CellPhone as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Region_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Region_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Department_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Department_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;
		  
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'OU_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.OU_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'CostCentre'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.CostCentre as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Place'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Place as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'UserCode'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.UserCode as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			if (@FieldId > @HeaderId + 1)
			begin
			set @FieldCaption = dbo.TextTranslate('Anmälare', @LanguageId);
 			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
						Values(@HeaderId, '[Title]', @FieldCaption, '', @HeaderInOrder, 'G');
			
			set @HeaderId = @FieldId;
			set @HeaderInOrder = @InOrder;	
			end
			else
			begin
			set  @FieldId = @FieldId - 1;
			set  @InOrder = @InOrder - 1;	
			end
		End
	  
		Begin /* Is About information  */
	      		  			  
			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_ReportedBy'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_ReportedBy as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Persons_Name'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Persons_Name as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Persons_EMail'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Persons_EMail as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;
		  
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Persons_Phone'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Persons_Phone as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Persons_CellPhone'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Persons_CellPhone as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Region_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Region_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Department_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Department_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;
		  
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_OU_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_OU_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_CostCentre'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_CostCentre as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_Place'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_Place as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'IsAbout_UserCode'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.IsAbout_UserCode as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			if (@FieldId > @HeaderId + 1)
			begin
				set @FieldCaption = dbo.TextTranslate('Angående', @LanguageId);
 				Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 							
						Values(@HeaderId, '[Title]', @FieldCaption, '', @HeaderInOrder, 'G');
			
			set @HeaderId = @FieldId;
			set @HeaderInOrder = @InOrder;	
			end
			else
			begin
			set  @FieldId = @FieldId - 1;
			set  @InOrder = @InOrder - 1;	
			end
		End
	
		Begin /* Computer information  */
		  		  							
			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'InventoryNumber'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.InventoryNumber as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ComputerType_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.ComputerType_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'InventoryLocation'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.InventoryLocation as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;
		  
			if (@FieldId > @HeaderId + 1)
			begin
				set @FieldCaption = dbo.TextTranslate('Datorinformation', @LanguageId);
 				Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
						Values(@HeaderId, '[Title]', @FieldCaption, '', @HeaderInOrder, 'G');
			
			set @HeaderId = @FieldId;
			set @HeaderInOrder = @InOrder;	
			end
			else
			begin
			set  @FieldId = @FieldId - 1;
			set  @InOrder = @InOrder - 1;	
			end		  		  		 
		End

		Begin /* Case information  */
		   		  
			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'RegTime'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Convert(varchar(10), c.RegTime, 120), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ChangeTime'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Convert(varchar(10), c.ChangeTime, 120), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'User_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.User_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'RegistrationSourceCustomer'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.RegistrationSourceCustomer as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		  		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'CaseType_id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.CaseType_id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ProductArea_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.ProductArea_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'System_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.System_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Urgency_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Urgency_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Impact_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Impact_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Category_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Category_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Supplier_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Supplier_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'InvoiceNumber'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.InvoiceNumber as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ReferenceNumber'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.ReferenceNumber as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Caption'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Caption as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Description'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Description as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Miscellaneous'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Miscellaneous as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ContactBeforeAction'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.ContactBeforeAction as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;				 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'SMS'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.SMS as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'AgreedDate'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Convert(varchar(10), c.AgreedDate , 120), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Available'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Available as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		
		  
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Cost'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Cost as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		
		  		 
				 
			/* Case Files */ 
			declare @FileNums int
			declare @ii int

			SELECT @FileNums = Count(id)
			FROM   tblCaseFile
			Where Case_Id = @CaseId						

			if (@FileNums > 0)
			begin
				/* Curson defination */
				
				Declare @CaseFileName  Nvarchar(Max)
				DECLARE CaseFile_Cursor CURSOR FOR 			
		
				SELECT [FileName] as CaseFile 
				FROM   tblCaseFile
				Where Case_Id = @CaseId	
				Order by CreatedDate						
		
				OPEN CaseFile_Cursor

				FETCH NEXT FROM CaseFile_Cursor 
				INTO @CaseFileName;
			
				set @ii = 0
				
				declare @AllFileNames Nvarchar(Max)
				set @AllFileNames = ''
				WHILE (@ii < @FileNums)
				BEGIN	 	    
					set @ii = @ii + 1		
					set @AllFileNames = @AllFileNames + CHAR(13) + CHAR(10) + @CaseFileName; 					
			
					if (@ii < @FileNums)
					begin
						FETCH NEXT FROM CaseFile_Cursor 
						INTO @CaseFileName;
					end

				End; /* While CaseFile_Cursor */ 
	
				CLOSE CaseFile_Cursor;
				DEALLOCATE CaseFile_Cursor;

				Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Filename'
				If (@FieldName is not null and @FieldName <> '')
				Begin    
					set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
					Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
						Select @FieldId, @FieldName, @FieldCaption, @AllFileNames, @InOrder, 'F'		
						From @CaseTable c;				 

					Set @FieldId = @FieldId + 1;
					Set @InOrder = @InOrder + 1;
				end
				
			End -- Case Files


			if (@FieldId > @HeaderId + 1)
			begin
				set @FieldCaption = dbo.TextTranslate('Ärendeinformation', @LanguageId);
 				Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
						Values(@HeaderId, '[Title]', @FieldCaption, '', @HeaderInOrder, 'G');
			
			set @HeaderId = @FieldId;
			set @HeaderInOrder = @InOrder;	
			end
			else
			begin
			set  @FieldId = @FieldId - 1;
			set  @InOrder = @InOrder - 1;	
			end
		End
	 

	 Declare @Nums int;
	 declare @i int;

	set @Nums = 0;

	 /* Self Service Fields */

	declare @SSTemplateName Nvarchar(Max)

	SELECT @Nums = Count(*), @SSTemplateName = tblForm.FormName 	
	FROM   tblFormFieldValue INNER JOIN
           tblCase ON tblFormFieldValue.Case_Id = tblCase.Id INNER JOIN
           tblFormField INNER JOIN
           tblForm ON tblFormField.Form_Id = tblForm.Id ON tblFormFieldValue.FormField_Id = tblFormField.Id
	Where Case_Id = @CaseId
	Group by tblForm.FormName
			
	if (@Nums > 0)
	begin
		/* Curson defination */
		Declare @SSFieldName     Nvarchar(Max);
		Declare @SSFieldCaption  Nvarchar(Max);
		Declare @SSFieldValue    Nvarchar(Max);
		
		DECLARE SelfService_Cursor CURSOR FOR 			
		

		SELECT 
		       tblFormField.FormFieldName as FieldName, tblFormField.Label as FieldCaption, 
			   tblFormFieldValue.FormFieldValue as FieldValue
		FROM   tblFormFieldValue INNER JOIN
           tblCase ON tblFormFieldValue.Case_Id = tblCase.Id INNER JOIN
           tblFormField INNER JOIN
           tblForm ON tblFormField.Form_Id = tblForm.Id ON tblFormFieldValue.FormField_Id = tblFormField.Id
		Where Case_Id = @CaseId
		Order by tblFormField.SortOrder 		

		declare @STitle Nvarchar(Max)
		
		set @STitle = dbo.TextTranslate('Utökad ärendeinformation', @LanguageId);

		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
					values(@FieldId, '[Title]', @STitle, @SSTemplateName, @InOrder, 'G')	  	 

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;

		OPEN SelfService_Cursor

		FETCH NEXT FROM SelfService_Cursor 
		INTO @SSFieldName, @SSFieldCaption, @SSFieldValue;
			
		set @i = 0
			
		WHILE (@i < @Nums)
		BEGIN	 	    
			set @i = @i + 1		
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				   values(@FieldId, @SSFieldName, @SSFieldCaption, @SSFieldValue, @InOrder, 'S')	  	 

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			
			if (@i < @Nums)
			begin
				FETCH NEXT FROM SelfService_Cursor 
				INTO @SSFieldName, @SSFieldCaption, @SSFieldValue;						
			end

		End; /* While SelfService_Cursor */ 
	
		CLOSE SelfService_Cursor;
		DEALLOCATE SelfService_Cursor;

	End -- Self Service Fields

	 Begin /*Case management information  */	    
	
			set @HeaderId = @FieldId;
			set @HeaderInOrder = @InOrder;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'WorkingGroup_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.WorkingGroup_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'CaseResponsibleUser_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.CaseResponsibleUser_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Performer_User_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Performer_User_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		  		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Priority_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Priority_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Status_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Status_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'StateSecondary_Id'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.StateSecondary_Id as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		  		 

		  
			/*  Projekt Show by Customer Setting */ 
			If (exists(select ModuleProject from tblSettings where id = 1 and ModuleProject = 1))
			Begin    
			set @FieldCaption = dbo.TextTranslate('Projekt', @LanguageId)
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, 'Project', @FieldCaption, Cast(c.Project as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			/*  Problem Show by Customer Setting */ 
			If (exists(select ModuleProblem from tblSettings where id = 1 and ModuleProblem = 1))
			Begin    
			set @FieldCaption = dbo.TextTranslate('ITIL Problem', @LanguageId)
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, 'Problem', @FieldCaption, Cast(c.Problem as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'CausingPart'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.CausingPart as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			/*  Change Show by Customer Setting */ 
			If (exists(select ModuleChangeManagement from tblSettings where id = 1 and ModuleChangeManagement = 1))
			Begin    
			set @FieldCaption = dbo.TextTranslate('Ändring', @LanguageId)
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, 'Change', @FieldCaption, Cast(c.Change as Nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'PlanDate'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Convert(nvarchar(10), c.PlanDate ,120), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		  		 

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'WatchDate'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Convert(nvarchar(10), c.WatchDate ,120), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		  

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'Verified'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.Verified as nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		
		  
			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'VerifiedDescription'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.VerifiedDescription as nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;		

			Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'SolutionRate'
			If (@FieldName is not null and @FieldName <> '')
			Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Cast(c.SolutionRate as nvarchar(Max)), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
			End;
		  
			if (@FieldId > @HeaderId + 1)
			begin
				set @FieldCaption = dbo.TextTranslate('Ärendehantering', @LanguageId);
 				Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
						Values(@HeaderId, '[Title]', @FieldCaption, '', @HeaderInOrder, 'G');
			
				set @HeaderId = @FieldId;
				set @HeaderInOrder = @InOrder;	
			end	
			else
			begin
				set  @FieldId = @FieldId - 1;
				set  @InOrder = @InOrder - 1;	
			end	
		End
	 	
	End

	Begin /* Closing information  */        		  		

		Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'ClosingReason'
		If (@FieldName is not null and @FieldName <> '')
		Begin    
		set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
			Select @FieldId, @FieldName, @FieldCaption, Cast(c.ClosingReason as Nvarchar(Max)), @InOrder, 'R'		
			From @CaseTable c;

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;
		Set @FieldName = '';
		End;

		Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'FinishingDate'
		If (@FieldName is not null and @FieldName <> '')
		Begin    
			set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
			Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				Select @FieldId, @FieldName, @FieldCaption, Convert(varchar(10), c.FinishingDate,120), @InOrder, 'R'		
				From @CaseTable c;

			Set @FieldId = @FieldId + 1;
			Set @InOrder = @InOrder + 1;
			Set @FieldName = '';
		End;

		Select @FieldName = FieldName, @FieldCaption = FieldCaption From @AvailableFields where FieldName = 'FinishingDescription'
		If (@FieldName is not null and @FieldName <> '')
		Begin    
		set @FieldCaption = Isnull(@FieldCaption, dbo.GetDefCaseFieldCaption(@FieldName, @LanguageId))   
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
			Select @FieldId, @FieldName, @FieldCaption, Cast(c.FinishingDescription as Nvarchar(Max)), @InOrder, 'R'		
			From @CaseTable c;

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;
		Set @FieldName = '';
		End;
		  
		if (@FieldId > @HeaderId + 1)
		begin				   
			set @HeaderId = @FieldId;
			set @HeaderInOrder = @InOrder;	
		end	
		else
		begin
			set  @FieldId = @FieldId - 1;
			set  @InOrder = @InOrder - 1;	
		end	  		 
	End
	

	/*  Log data */    		 
	Declare @ExternalFieldName  Nvarchar(Max);
	Declare @ExternalFieldCaption  Nvarchar(Max);
	Declare @InternalFieldName  Nvarchar(Max);
	Declare @InternalFieldCaption  Nvarchar(Max);
	Declare @InvoiceTimeFieldCaption Nvarchar(Max);
	
	Declare @Log_Id int;
	Declare @Log_LogDate datetime;
	Declare @Log_Text_External Nvarchar(Max);
	Declare @Log_Text_Internal Nvarchar(Max);
	Declare @Log_UserName nvarchar(Max);	
	Declare @Log_Value nvarchar(Max);	

	/*InvoiceTime*/
	Declare @Log_WorkingTime int;
	Declare @Log_OverTime int;
	Declare @Log_EquipmentPrice money;
	Declare @Log_Price int;	
	Declare @Log_InvoiceStatus int;	

	Declare @NewLine nvarchar(5)
	
	Set @NewLine = char(13) + char(10)
	 
	SELECT @Nums = Count(*) 
	FROM tblLog l 
		Left outer Join tblUsers u on (l.User_Id = u.Id),
		tblSettings s		 
	WHERE l.Case_Id = @CaseId and s.Customer_Id = @CurrentCustomerId and (l.Text_External <> '' or l.Text_Internal <> '')			

	/* Curson defination */
	DECLARE logData_Cursor CURSOR SCROLL FOR 				
	SELECT l.Id, l.LogDate, cast(l.Text_External as nvarchar(max)), cast(l.Text_Internal as nvarchar(max)), 
			Case when s.IsUserFirstLastNameRepresentation = 1 then
				cast(u.FirstName + ' ' + u.SurName as nvarchar(max))
			else
				cast(u.SurName + ' ' + u.FirstName as nvarchar(max))
			End as UserName,
			l.WorkingTime, l.OverTime, l.EquipmentPrice,l.Price, isnull(ir.Status,1) as InvoiceStatus
	FROM tblLog l 
		Left outer Join tblInvoiceRow ir on (l.InvoiceRow_Id = ir.Id)
		Left outer Join tblUsers u on (l.User_Id = u.Id),
		tblSettings s		 
	WHERE l.Case_Id = @CaseId and s.Customer_Id = @CurrentCustomerId and (l.Text_External <> '' or l.Text_Internal <> '')
	ORDER BY l.Id desc;

	

	OPEN logData_Cursor

	FETCH NEXT FROM logData_Cursor 
	INTO @Log_Id, @Log_LogDate, @Log_Text_External,
			@Log_Text_Internal, @Log_UserName, @Log_WorkingTime,
			@Log_OverTime, @Log_EquipmentPrice,@Log_Price,
			@Log_InvoiceStatus;			
		
		
	Set @ExternalFieldName  = '';
	Select @ExternalFieldName = FieldName, @ExternalFieldCaption = FieldCaption From @AvailableFields where FieldName = 'tblLog.Text_External';
	Set @ExternalFieldCaption = Isnull(@ExternalFieldCaption, dbo.GetDefCaseFieldCaption('tbllog_text_external', @LanguageId)) 

	Set @InternalFieldName  = '';
	Select @InternalFieldName = FieldName, @InternalFieldCaption = FieldCaption From @AvailableFields where FieldName = 'tblLog.Text_Internal';	      	 
	Set @InternalFieldCaption = Isnull(@InternalFieldCaption, dbo.GetDefCaseFieldCaption('tbllog_text_internal', @LanguageId)) 
				
	Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
			         values(@FieldId, '[Title]', dbo.TextTranslate('Ärendelogg', @LanguageId), '', @InOrder, 'G')	  	 	

	Set @FieldId = @FieldId + 1;
	Set @InOrder = @InOrder + 1;	  
	Set @Log_Value = '';

	
	set @i = 0
					
	WHILE (@i < @Nums)
	BEGIN	 	    
		set @i = @i + 1		
		declare @logLocalTime DateTime;
		set @logLocalTime = DATEADD(mi, @UserTimeOffset, @Log_LogDate); 

		set @Log_Value = CONVERT(VARCHAR(19), @logLocalTime, 120);		
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				values(@FieldId, 'LogNote', @Log_Value, @Log_UserName, @InOrder, 'L')	  	 

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;	  
		Set @Log_Value = '';

		if (@Log_Text_External <> '' or @Log_Text_Internal <> '')
		begin		  		  
			if (@Log_Text_External <> '')
			begin
				set @Log_Value = @Log_Text_External;
				Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
								values(@FieldId, 'External-LogNote', @ExternalFieldCaption, @Log_Value, @InOrder, 'L')	  	 

				Set @FieldId = @FieldId + 1;
				Set @InOrder = @InOrder + 1;	  
				Set @Log_Value = '';
			End

			if (@Log_Text_Internal <> '')
			Begin
				set @Log_Value = @Log_Text_Internal;	  	  
				Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
								values(@FieldId, 'Internal-LogNote', @InternalFieldCaption, @Log_Value, @InOrder, 'L')	  	 

				Set @FieldId = @FieldId + 1;
				Set @InOrder = @InOrder + 1;	  
				Set @Log_Value = '';
			end		  
		end;		

		if (@i < @Nums)
		begin
			FETCH NEXT FROM logData_Cursor 
			INTO @Log_Id, @Log_LogDate, @Log_Text_External,
				 @Log_Text_Internal, @Log_UserName, @Log_WorkingTime,
				 @Log_OverTime, @Log_EquipmentPrice,@Log_Price,
				 @Log_InvoiceStatus;						
		end

	End; /* While Log_Cursor */ 
	

	/*Invoice Time if exists*/
	Declare @InvoiceRowsNum int
	SELECT @InvoiceRowsNum = Count(*) 
	FROM tblLog l 		
	WHERE l.Case_Id = @CaseId and (l.Text_External <> '' or l.Text_Internal <> '') and 
		  (l.WorkingTime > 0 or l.OverTime > 0 or l.EquipmentPrice > 0 or l.Price > 0)

	if (@InvoiceRowsNum > 0)
	begin
		fetch first from logData_Cursor
		INTO @Log_Id, @Log_LogDate, @Log_Text_External,
				@Log_Text_Internal, @Log_UserName, @Log_WorkingTime,
				@Log_OverTime, @Log_EquipmentPrice, @Log_Price,
				@Log_InvoiceStatus;

		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
			         values(@FieldId, '[Title]', dbo.TextTranslate('Fakturering', @LanguageId), '', @InOrder, 'IG')

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;	  
		Set @Log_Value = '';

		declare @InvoiceDate_Caption nvarchar(50);
		declare @InvoiceText_Caption nvarchar(50);
		declare @InvoiceWorkingTime_Caption nvarchar(50);
		declare @InvoiceOverTime_Caption nvarchar(50);
		declare @InvoiceEquipmentPrice_Caption nvarchar(50);
		declare @InvoicePrice_Caption nvarchar(50);
		declare @InvoiceStatus_Caption nvarchar(50);
		declare @_Time_Translation nvarchar(50);

		Set @_Time_Translation = dbo.TextTranslate('timmar', @LanguageId);

		Set @InvoiceDate_Caption =  dbo.TextTranslate('Datum', @LanguageId);
		Set @InvoiceText_Caption = dbo.TextTranslate('Text', @LanguageId);
		Set @InvoiceWorkingTime_Caption = dbo.TextTranslate('Arbete', @LanguageId) + ' ' + @_Time_Translation;
		Set @InvoiceOverTime_Caption = dbo.TextTranslate('Övertid', @LanguageId)+ ' ' + @_Time_Translation;
		Set @InvoiceEquipmentPrice_Caption = dbo.TextTranslate('Material', @LanguageId);
		Set @InvoicePrice_Caption = dbo.TextTranslate('Pris', @LanguageId);
		Set @InvoiceStatus_Caption = dbo.TextTranslate('Status', @LanguageId);
				
		Set @InvoiceTimeFieldCaption = 'Invoice Times' 

		Set @Log_Value = ' [ ' +
					'{' + 
						' "Date": "' + @InvoiceDate_Caption + '"' + 
						', "Text": "' + @InvoiceText_Caption + '"' + 
						', "WorkingTime": "' + @InvoiceWorkingTime_Caption + '"' + 
						', "OverTime": "' + @InvoiceOverTime_Caption + '"' + 
						', "EquipmentPrice": "' + @InvoiceEquipmentPrice_Caption + '"' + 
						', "Price": "' + @InvoicePrice_Caption + '"' + 
						', "InvoiceStatus": "' + @InvoiceStatus_Caption + '"' + 
					 '}';

		set @i = 0
					
		WHILE (@i < @Nums)
		BEGIN	 	    
			set @i = @i + 1		
			declare @logInvoiceLocalTime DateTime;
			set @logInvoiceLocalTime = DATEADD(mi, @UserTimeOffset, @Log_LogDate); 

			declare @logInvoiceTimeStr nvarchar(max)
			set @logInvoiceTimeStr = CONVERT(VARCHAR(10), @logInvoiceLocalTime, 120);					

			/*Create InvoiceTime row*/
			if (@Log_WorkingTime > 0 or @Log_OverTime > 0 or @Log_EquipmentPrice > 0 or @Log_Price > 0)
			begin		
				declare @curText Nvarchar(Max)	
				if (@Log_Text_Internal <> '')
					set @curText = @Log_Text_Internal
				else
					set @curText = @Log_Text_External

				Set @Log_Value = @Log_Value +
					', {' + 
						' "Date": "' + @logInvoiceTimeStr + '"' + 
						', "Text": "' + replace(replace(@curText,'\','\\'),'"','\"') + '"' + 
						', "WorkingTime": ' + cast(@Log_WorkingTime as nvarchar(max)) + 
						', "OverTime": ' + cast(@Log_OverTime as nvarchar(max)) + 
						', "EquipmentPrice": ' + cast(@Log_EquipmentPrice as nvarchar(max)) + 
						', "Price": ' + cast(@Log_Price as nvarchar(max)) + 
						', "InvoiceStatus": ' + cast(isnull(@Log_InvoiceStatus,1) as nvarchar(max)) + 
					  '}';				
			End;

			if (@i < @Nums)
			begin
				FETCH NEXT FROM logData_Cursor 
				INTO @Log_Id, @Log_LogDate, @Log_Text_External,
					 @Log_Text_Internal, @Log_UserName, @Log_WorkingTime,
					 @Log_OverTime, @Log_EquipmentPrice,@Log_Price,
					 @Log_InvoiceStatus;						
			end

		End; /* While Log_Cursor */

		Set @Log_Value = @Log_Value + ' ] ';
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
					values(@FieldId, 'Invoice', @InvoiceTimeFieldCaption, @Log_Value, @InOrder, 'I')	 	

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;	  
		Set @Log_Value = '';

	end /* If there is any Invoice row */

	CLOSE logData_Cursor;
	DEALLOCATE logData_Cursor;

	/**********************************************************************/
	/*  Invoice Rows*/    		 		
	Declare @IR_InvoiceDate datetime;
	Declare @IR_InvoiceNumber Nvarchar(50);
	Declare @IR_InvoicePrice numeric(9,2);	
	Declare @IR_InvoiceStatus int;	
	 
	SELECT @Nums = Count(*) 
	FROM tblCaseInvoiceRow
	WHERE Case_Id = @CaseId
	
	if (@Nums > 0)
	begin
				
		/* Curson defination */
		DECLARE invoiceRow_Cursor CURSOR FOR 				
			SELECT ir.CreatedDate, ir.InvoiceNumber, ir.InvoicePrice, isnull(tir.Status,1) as InvoiceStatus
			FROM tblCaseInvoiceRow ir 	
				 Left outer Join tblInvoiceRow tir on (ir.InvoiceRow_Id = tir.Id)		
			WHERE ir.Case_Id = @CaseId
			ORDER BY ir.Id desc;		

		OPEN invoiceRow_Cursor
		FETCH NEXT FROM invoiceRow_Cursor 
		INTO @IR_InvoiceDate, @IR_InvoiceNumber, 
			 @IR_InvoicePrice, @IR_InvoiceStatus;									

		declare @IRDate_Caption nvarchar(50);
		declare @IRNumber_Caption nvarchar(50);
		declare @IRPrice_Caption nvarchar(50);
		declare @IRStatus_Caption nvarchar(50);	

		Set @IRDate_Caption =  dbo.TextTranslate('Datum', @LanguageId);
		Set @IRNumber_Caption = dbo.TextTranslate('Fakturanummer', @LanguageId);
		Set @IRPrice_Caption = dbo.TextTranslate('Fakturapris', @LanguageId);
		Set @IRStatus_Caption = dbo.TextTranslate('Status', @LanguageId);

		Set @Log_Value = ' [ ' +
					'{' + 
						' "Date": "' + @IRDate_Caption + '"' + 
						', "InvoiceNumber": "' + @IRNumber_Caption + '"' + 
						', "InvoicePrice": "' + @IRPrice_Caption + '"' +						
						', "InvoiceStatus": "' + @IRStatus_Caption + '"' + 
					 '}';
		
		set @i = 0					
		WHILE (@i < @Nums)
		BEGIN	 	    
			set @i = @i + 1		
			declare @invoiceRowLocalTime DateTime;
			set @invoiceRowLocalTime = DATEADD(mi, @UserTimeOffset, @IR_InvoiceDate); 
			
			declare @invoiceRowLocalTimeStr nvarchar(10)
			set @invoiceRowLocalTimeStr = CONVERT(VARCHAR(10), @invoiceRowLocalTime, 120);					

			/*Create Invoice Row*/								
			Set @Log_Value = @Log_Value +
					', {' + 
						 ' "Date": "' + @invoiceRowLocalTimeStr + '"' + 
						 ', "InvoiceNumber": "' +  replace(replace(isnull(@IR_InvoiceNumber,''),'\','\\'),'"','\"')  + '"' + 
						 ', "InvoicePrice": "' + cast(@IR_InvoicePrice as nvarchar(20)) + '"' +						
						 ', "InvoiceStatus": "' + cast(isnull(@IR_InvoiceStatus,1) as nvarchar(20))+ '"' +  
					   '}';				
			
			if (@i < @Nums)
			begin
				FETCH NEXT FROM invoiceRow_Cursor 
				INTO @IR_InvoiceDate, @IR_InvoiceNumber, 
					 @IR_InvoicePrice, @IR_InvoiceStatus;						
			end

		End; /* While InvocieRow_Cursor */ 

		Set @Log_Value = @Log_Value + ' ] ';		
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
					values(@FieldId, 'Invoice', 'InvoiceRows', @Log_Value, @InOrder, 'IR')	 	

		Set @FieldId = @FieldId + 1;
		Set @InOrder = @InOrder + 1;	  
		Set @Log_Value = '';

		CLOSE invoiceRow_Cursor;
		DEALLOCATE invoiceRow_Cursor;

	end -- External Invoice Row

	if (@FieldId > @HeaderId + 1)
	begin			    			
		set @HeaderId = @FieldId;
		set @HeaderInOrder = @InOrder;	
	end
	else
	begin
		set  @FieldId = @FieldId - 1;
		set  @InOrder = @InOrder - 1;	
	end
	

	select * from @ResultSet order by InOrder Asc

Go




IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'ShowInvoice' and sysobjects.name = N'tblOU')
begin
	ALTER TABLE [dbo].[tblOU] ADD [ShowInvoice] bit NOT NULL Default(0)
end


IF NOT exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id 
               where syscolumns.name = N'DefaultCaseTemplateId' and sysobjects.name = N'tblSettings')
begin
	ALTER TABLE [dbo].[tblSettings] ADD DefaultCaseTemplateId int NOT NULL Default(0)
end


--#59777 Quickopen

  if not exists(Select * from tblModule where id = 13) 
  begin
	SET IDENTITY_INSERT tblModule ON

	INSERT tblModule (Id, Name, [Description]) VALUES (13, 'Snabbåtkomst', 'Snabbåtkomst')  
	
	SET IDENTITY_INSERT tblModule OFF

	end


--#59677 - Multi case split


if not exists(select * from sysobjects WHERE Name = N'tblConditionType')
begin

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblConditionType](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Guid] [uniqueidentifier] NULL,
		[Name] [nvarchar](200) NULL,
		[Status] [int] NULL,
		[SortOrder] [int] NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_tblConditionType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	ALTER TABLE [dbo].[tblConditionType] ADD  CONSTRAINT [DF_tblConditionType_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]


	ALTER TABLE [dbo].[tblConditionType] ADD  CONSTRAINT [DF_tblConditionType_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]


	--insert content to [tblConditionType]
	
	Declare @ConditionTypeGuid uniqueidentifier = '1DB0EC37-3EFF-4270-B22A-5707C03F1E61'
	Declare @ConditionTypeId int = 1
	
	if not exists  (select * from [tblConditionType] where [Guid] = @ConditionTypeGuid)
	begin
		SET IDENTITY_INSERT [tblConditionType] ON

		insert into [tblConditionType] (Id, [Guid], Name, [Status], [SortOrder]) VALUES (@ConditionTypeId,@ConditionTypeGuid, 'Multicase - Split', 1, 0)

		SET IDENTITY_INSERT [tblCaseSolutionType] OFF

	end
	else
	begin
		update [tblCaseSolutionType] set Name = 'Multicase - Split', [Status] = 1, [SortOrder] = 0 where [Guid] = @ConditionTypeGuid
	end

end


if not exists(select * from sysobjects WHERE Name = N'tblCondition')
begin

SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCondition](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[GUID] [uniqueidentifier] NULL,
		[ConditionType_Id] [int] NULL,
		[Name] [nvarchar](200) NULL,
		[Description] [nvarchar](200) NULL,
		[Parent_Id] [int] NULL,
		[Property_Name] [nvarchar](500) NULL,
		[Operator] int NOT NULL,
		[Values] [nvarchar](max) NULL,
		[SortOrder] [int] NOT NULL,
		[Status] [int] NOT NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_Condition] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


	ALTER TABLE [dbo].[tblCondition] ADD  CONSTRAINT [DF_tblCondition_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]

	ALTER TABLE [dbo].[tblCondition] ADD  CONSTRAINT [DF_tblCondition_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

	ALTER TABLE [dbo].[tblCondition] ADD  CONSTRAINT [DF_tblCondition_Operator]  DEFAULT (0) FOR [Operator]

	ALTER TABLE [dbo].[tblCondition]  WITH CHECK ADD  CONSTRAINT [FK_tblCondition_tblConditionType] FOREIGN KEY([ConditionType_Id])
	REFERENCES [dbo].[tblConditionType] ([Id])

	ALTER TABLE [dbo].[tblCondition] CHECK CONSTRAINT [FK_tblCondition_tblConditionType]
end



if not exists(select * from sysobjects WHERE Name = N'tblCaseSolutionType')
begin

	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	CREATE TABLE [dbo].[tblCaseSolutionType](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Guid] [uniqueidentifier] NULL,
		[Name] [nvarchar](200) NULL,
		[Status] [int] NOT NULL,
		[SortOrder] [int] NOT NULL,
		[CreatedDate] [datetime] NULL,
		[CreatedByUser_Id] [int] NULL,
		[ChangedDate] [datetime] NULL,
		[ChangedByUser_Id] [int] NULL,
	 CONSTRAINT [PK_tblCaseSolutionType] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]



	ALTER TABLE [dbo].[tblCaseSolutionType] ADD  CONSTRAINT [DF_tblCaseSolutionType_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]


	ALTER TABLE [dbo].[tblCaseSolutionType] ADD  CONSTRAINT [DF_tblCaseSolutionType_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]

	--insert content to [tblCaseSolutionType]
	Declare @Guid uniqueidentifier = '266EBECD-C55A-4848-A980-1EEEFF13E9B7'
	Declare @Id int = 1
	
	if not exists  (select * from [tblCaseSolutionType] where [Guid] = @Guid)
	begin
		SET IDENTITY_INSERT [tblCaseSolutionType] ON

		insert into [tblCaseSolutionType] (Id, [Guid], Name, [Status], [SortOrder]) VALUES (@Id,@Guid, 'User - CaseSolution', 1, 0)

		SET IDENTITY_INSERT [tblCaseSolutionType] OFF

	end
	else
	begin
		update [tblCaseSolutionType] set Name = 'User - CaseSolution', [Status] = 1, [SortOrder] = 0 where [Guid] = @Guid
	end

	

end

if not exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id  where syscolumns.name = N'CaseSolutionType_Id' and sysobjects.name = N'tblCaseSolution')
begin

	--refer to table above!
	ALTER TABLE [tblCaseSolution] ADD [CaseSolutionType_Id]  int NOT NULL DEFAULT(1) 

end



-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.35'