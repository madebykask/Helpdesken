-- update DB from 5.3.21 to 5.3.22 version


IF COL_LENGTH('tblCaseSolution','Status') IS NULL
begin
    alter table tblCaseSolution 
	add [Status] int not null default(1)
end
GO

if exists(select * from sys.default_constraints where name like 'DF_tblCausingPart_CreatedDate')
Begin
  alter table tblCausingPart
  drop constraint DF_tblCausingPart_CreatedDate
end
GO


IF OBJECT_ID('dbo.GetHierarchy') IS NOT NULL
  DROP FUNCTION dbo.GetHierarchy
GO

Create function dbo.GetHierarchy (@Id int, @TableName Nvarchar(200))	
Returns NVARCHAR(3000) 
WITH EXECUTE AS CALLER
AS
BEGIN   
	DECLARE @tempTable Table(RowNum int, Value Nvarchar(3000));	
	DECLARE @FullName NVARCHAR(3000);

	If (@TableName = 'tblCaseType')
	Begin
		with CaseType_Tree as (
			select id, Parent_CaseType_Id, CaseType
			from tblCaseType
			where id = @Id
			union all
			select c.id, c.Parent_CaseType_Id, c.CaseType
			from tblCaseType c
				join CaseType_Tree p on p.Parent_CaseType_Id = c.id 
		) 
	
		Insert into @tempTable (RowNum,Value)
			Select ROW_NUMBER() OVER (order by (select count(*) from CaseType_Tree)), CaseType from CaseType_Tree;
	End	
	Else
	If (@TableName = 'tblProductArea')
	Begin
		with ProductArea_Tree as (
			select id, Parent_ProductArea_Id, ProductArea
			from tblProductArea
			where id = @Id
			union all
			select pa.id, pa.Parent_ProductArea_Id, pa.ProductArea
			from tblProductArea pa
				join ProductArea_Tree p on p.Parent_ProductArea_Id = pa.id 
		) 
	
		Insert into @tempTable (RowNum,Value)
			Select ROW_NUMBER() OVER (order by (select count(*) from ProductArea_Tree)), ProductArea from ProductArea_Tree;
	End
	Else
	If (@TableName = 'tblFinishingCause')
	Begin
		with FinishingCause_Tree as (
			select id, Parent_FinishingCause_Id, FinishingCause
			from tblFinishingCause
			where id = @Id
			union all
			select fc.id, fc.Parent_FinishingCause_Id, fc.FinishingCause
			from tblFinishingCause fc
				join FinishingCause_Tree p on p.Parent_FinishingCause_Id = fc.id 
		) 
			
		Insert into @tempTable (RowNum,Value)
			Select ROW_NUMBER() OVER (order by (select count(*) from FinishingCause_Tree)), FinishingCause from FinishingCause_Tree;
	End
	Else
	If (@TableName = 'tblOU')
	Begin
		with OU_Tree as (
			select id, Parent_OU_Id, OU
			from tblOU
			where id = @Id
			union all
			select o.id, o.Parent_OU_Id, o.OU
			from tblOU o
				join OU_Tree p on p.Parent_OU_Id = o.Id 
		) 
			
		Insert into @tempTable (RowNum,Value)
			Select ROW_NUMBER() OVER (order by (select count(*) from OU_Tree)), OU from OU_Tree;
	End
	Else	
	If (@TableName = 'tblCausingPart')
	Begin
		with CausingPart_Tree as (
			select id, Parent_CausingPart_Id, Name
			from tblCausingPart
			where id = @Id
			union all
			select c.id, c.Parent_CausingPart_Id, c.Name
			from tblCausingPart c
				join CausingPart_Tree p on p.Parent_CausingPart_Id = c.Id 
		) 
			
		Insert into @tempTable (RowNum,Value)
			Select ROW_NUMBER() OVER (order by (select count(*) from CausingPart_Tree)), Name from CausingPart_Tree;
	End
	Else
		Return 'Table "' + @TableName + '" is not defined!'		
	

	SELECT @FullName = COALESCE(@FullName + ' - ', '') + Value 
	FROM @tempTable 
	Order by RowNum desc;	
    
	return @FullName;
End
Go

IF OBJECT_ID('dbo.TextTranslate') IS NOT NULL
  DROP FUNCTION dbo.TextTranslate
GO

Create Function dbo.TextTranslate(@text Nvarchar(Max), @languageId int)
	Returns NVARCHAR(Max) 
	WITH EXECUTE AS CALLER
AS
BEGIN
	Declare @TranslatedText Nvarchar(Max);
	set @TranslatedText = '';
	Select @TranslatedText = tt.TextTranslation 
	from tblText t 
		inner join tblTextTranslation tt on (t.Id = tt.Text_Id and tt.Language_Id = @languageId)
	where t.TextString = @text;

	if (@TranslatedText is null or @TranslatedText = '')
	set @TranslatedText = @text;

	return @TranslatedText
End
Go

IF OBJECT_ID('dbo.GetDefCaseFieldCaption') IS NOT NULL
  DROP FUNCTION dbo.GetDefCaseFieldCaption
GO

Create Function dbo.GetDefCaseFieldCaption(@fieldName Nvarchar(200), @languageId int)
Returns NVARCHAR(Max) 
WITH EXECUTE AS CALLER
AS
BEGIN
	Declare @DefaultCaption Nvarchar(Max);
 
	if (@languageId = 1)
	Begin
	Select @DefaultCaption = 
		Case     			
			When @fieldName =  'agreeddate' then 'Överenskommet datum'
			When @fieldName =  'available' then 'Anträffbar'
			When @fieldName =  'When @fieldName = caption' then 'Rubrik'
			When @fieldName =  'caption' then 'Rubrik'
			When @fieldName =  'When @fieldName = number' then 'Ärende'
			When @fieldName =  'When @fieldName = responsibleuser_id' then 'Ansvarig'
			When @fieldName =  'When @fieldName = type_id' then 'Ärendetyp'
			When @fieldName =  'category_id' then 'Kategori'
			When @fieldName =  'changetime' then 'Ändringsdatum'
			When @fieldName =  'computertype_id' then 'Datortyp'
			When @fieldName =  'contactbeforeaction' then 'Telefonkontakt'
			When @fieldName =  'cost' then 'Kostnad'
			When @fieldName =  'customer_id' then 'Kund'
			When @fieldName =  'department_id' then 'Avdelning'
			When @fieldName =  'description' then 'Beskrivning'
			When @fieldName =  'filename' then 'Bifogad fil'
			When @fieldName =  'finishingdate' then 'Avslutsdatum'
			When @fieldName =  'finishingdescription' then 'Avslutsbeskrivning'
			When @fieldName =  'impact_id' then 'Påverkan'
			When @fieldName =  'inventorylocation' then 'Placering'
			When @fieldName =  'inventorynumber' then 'PC Nummer'
			When @fieldName =  'invoicenumber' then 'Fakturanummer'
			When @fieldName =  'miscellaneous' then 'Övrigt'
			When @fieldName =  'ou_id' then 'Enhet'
			When @fieldName =  'performer_user_id' then 'Handläggare'
			When @fieldName =  'persons_cellphone' then 'Mobilnr.'
			When @fieldName =  'persons_email' then 'E-post'
			When @fieldName =  'persons_name' then 'Namn'
			When @fieldName =  'persons_phone' then 'Telefon'
			When @fieldName =  'place' then 'Plats'
			When @fieldName =  'plandate' then 'Planerat datum'
			When @fieldName =  'priority_id' then 'Prioritet'
			When @fieldName =  'productarea_id' then 'Produktområde'
			When @fieldName =  'referencenumber' then 'Referensnummer'
			When @fieldName =  'region_id' then 'Område'
			When @fieldName =  'regtime' then 'Registreringsdatum'
			When @fieldName =  'reportedby' then 'Användar ID'
			When @fieldName =  'sms' then 'SMS'
			When @fieldName =  'solutionrate' then 'Lösningsgrad'
			When @fieldName =  'statesecondary_id' then 'Understatus'
			When @fieldName =  'status_id' then 'Status'
			When @fieldName =  'supplier_id' then 'Leverantör'
			When @fieldName =  'system_id' then 'System'
			When @fieldName =  'tbllog_charge' then 'Debitering'
			When @fieldName =  'tbllog_filename' then 'Log bifogad fil'
			When @fieldName =  'tbllog_text_external' then 'Extern notering'
			When @fieldName =  'tbllog_text_internal' then 'Intern notering'
			When @fieldName =  'tbllog.charge' then 'Debitering'
			When @fieldName =  'tbllog.filename' then 'Log bifogad fil'
			When @fieldName =  'tbllog.text_external' then 'Extern notering'
			When @fieldName =  'tbllog.text_internal' then 'Intern notering'
			When @fieldName =  'urgency_id' then 'Brådskandegrad'
			When @fieldName =  'user_id' then 'Registrerad av'
			When @fieldName =  'usercode' then 'Ansvarskod'
			When @fieldName =  'watchdate' then 'Bevakningsdatum'
			When @fieldName =  'verified' then 'Verifierad'
			When @fieldName =  'verifieddescription' then 'Verifierad beskrivning'
			When @fieldName =  'workinggroup_id' then 'Driftgrupp'
			When @fieldName =  'causingpart' then 'Rotorsak'
			When @fieldName =  'closingreason' then 'Avslutsorsak'
			When @fieldName =  '_temporary_.leadtime' then 'Tid kvar'
			When @fieldName =  'tblproblem.responsibleuser_id' then 'Problem'
			When @fieldName =  'change' then 'Ändringshantering'
			When @fieldName =  'project' then 'Projekt'
			When @fieldName =  'registrationsourcecustomer' then 'Källa'
			When @fieldName =  'isabout_reportedby' then 'Användar ID'
			When @fieldName =  'isabout_persons_name' then 'Namn'
			When @fieldName =  'isabout_persons_email' then 'E-post'
			When @fieldName =  'isabout_persons_phone' then 'Telefon'
			When @fieldName =  'isabout_persons_cellphone' then 'Mobilnr.'
			When @fieldName =  'isabout_region_id' then 'Område'
			When @fieldName =  'isabout_department_id' then 'Avdelning'
			When @fieldName =  'isabout_ou_id' then 'Enhet'
			When @fieldName =  'isabout_costcentre' then 'Kostnad'
			When @fieldName =  'isabout_place' then 'Plats'
			When @fieldName =  'isabout_usercode' then 'Ansvarskod'
			Else
				@fieldName
		End
	End
	Else
	Begin
	Select @DefaultCaption = 
		Case     			
			When @fieldName =  'agreeddate' then 'Agreed date'
			When @fieldName =  'available' then 'Available'
			When @fieldName =  'When @fieldName = caption' then 'Subject'
			When @fieldName =  'caption' then 'Subject'
			When @fieldName =  'When @fieldName = number' then 'When @fieldName = '
			When @fieldName =  'When @fieldName = responsibleuser_id' then 'Responsible'
			When @fieldName =  'When @fieldName = type_id' then 'When @fieldName =  Type'
			When @fieldName =  'category_id' then 'Category'
			When @fieldName =  'changetime' then 'Date of change'
			When @fieldName =  'computertype_id' then 'Computer type'
			When @fieldName =  'contactbeforeaction' then 'Phone contact'
			When @fieldName =  'cost' then 'Cost'
			When @fieldName =  'customer_id' then 'Customer'
			When @fieldName =  'department_id' then 'Department'
			When @fieldName =  'description' then 'Description'
			When @fieldName =  'filename' then 'Attached file'
			When @fieldName =  'finishingdate' then 'Closing date'
			When @fieldName =  'finishingdescription' then 'Closing description'
			When @fieldName =  'impact_id' then 'Impact'
			When @fieldName =  'inventorylocation' then 'Placement'
			When @fieldName =  'inventorynumber' then 'PC Nummer'
			When @fieldName =  'invoicenumber' then 'Invoice number'
			When @fieldName =  'miscellaneous' then 'Other'
			When @fieldName =  'ou_id' then 'Unit'
			When @fieldName =  'performer_user_id' then 'Administrator'
			When @fieldName =  'persons_cellphone' then 'Cell Phone'
			When @fieldName =  'persons_email' then 'E-mail'
			When @fieldName =  'persons_name' then 'Initiator'
			When @fieldName =  'persons_phone' then 'Phone'
			When @fieldName =  'place' then 'Placement'
			When @fieldName =  'plandate' then 'Planned action date'
			When @fieldName =  'priority_id' then 'Priority'
			When @fieldName =  'productarea_id' then 'Product area'
			When @fieldName =  'referencenumber' then 'Reference number'
			When @fieldName =  'region_id' then 'Region'
			When @fieldName =  'regtime' then 'Registration date'
			When @fieldName =  'reportedby' then 'User ID'
			When @fieldName =  'sms' then 'SMS'
			When @fieldName =  'solutionrate' then 'Resolution rate'
			When @fieldName =  'statesecondary_id' then 'Sub status'
			When @fieldName =  'status_id' then 'Status'
			When @fieldName =  'supplier_id' then 'Supplier'
			When @fieldName =  'system_id' then 'System'
			When @fieldName =  'tbllog_charge' then 'Charge'
			When @fieldName =  'tbllog_filename' then 'Log Attached file'
			When @fieldName =  'tbllog_text_external' then 'External log note'
			When @fieldName =  'tbllog_text_internal' then 'Internal log note'
			When @fieldName =  'tbllog.charge' then 'Charge'
			When @fieldName =  'tbllog.filename' then 'Log Attached file'
			When @fieldName =  'tbllog.text_external' then 'External log note'
			When @fieldName =  'tbllog.text_internal' then 'Internal log note'
			When @fieldName =  'urgency_id' then 'Urgent degree'
			When @fieldName =  'user_id' then 'Registered by'
			When @fieldName =  'usercode' then 'Orderer Code'
			When @fieldName =  'watchdate' then 'Watch date'
			When @fieldName =  'verified' then 'Verified'
			When @fieldName =  'verifieddescription' then 'Verified description'
			When @fieldName =  'workinggroup_id' then 'Working Group'
			When @fieldName =  'causingpart' then 'Causing part'
			When @fieldName =  'closingreason' then 'Closing reason'
			When @fieldName =  '_temporary_.leadtime' then 'Time left'
			When @fieldName =  'tblproblem.responsibleuser_id' then 'Problem'
			When @fieldName =  'change' then 'Change'
			When @fieldName =  'project' then 'Project'
			When @fieldName =  'registrationsourcecustomer' then 'Source'
			When @fieldName =  'isabout_reportedby' then 'User ID'
			When @fieldName =  'isabout_persons_name' then 'Initiator'
			When @fieldName =  'isabout_persons_email' then 'E-mail'
			When @fieldName =  'isabout_persons_phone' then 'Phone'
			When @fieldName =  'isabout_persons_cellphone' then 'Cell Phone'
			When @fieldName =  'isabout_region_id' then 'Region'
			When @fieldName =  'isabout_department_id' then 'Department'
			When @fieldName =  'isabout_ou_id' then 'Unit'
			When @fieldName =  'isabout_costcentre' then 'Cost'
			When @fieldName =  'isabout_place' then 'Placement'
			When @fieldName =  'isabout_usercode' then 'Orderer Code'
		Else
				@fieldName
		End
	End				
	return @DefaultCaption;	
End
GO

IF OBJECT_ID('dbo.sp_GetCaseInfo') IS NOT NULL
  DROP Proc dbo.[sp_GetCaseInfo]
GO

Create Proc dbo.[sp_GetCaseInfo] 
	@CaseId int, 	
	@LanguageId int,
	@UserId int
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
	 
		Begin /* Case management information  */	    
		  
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

	Declare @Log_Id int;
	Declare @Log_LogDate datetime;
	Declare @Log_Text_External Nvarchar(Max);
	Declare @Log_Text_Internal Nvarchar(Max);
	Declare @Log_UserName nvarchar(Max);	
	Declare @Log_Value nvarchar(Max);	
	Declare @NewLine nvarchar(5)
	
	Set @NewLine = char(13) + char(10)

	 /* Curson defination */
		DECLARE logData_Cursor CURSOR FOR 			
		SELECT l.Id, l.LogDate, l.Text_External, l.Text_Internal, 
				Case when s.IsUserFirstLastNameRepresentation = 1 then
					cast(u.FirstName + ' ' + u.SurName as nvarchar(max))
				else
					cast(u.SurName + ' ' + u.FirstName as nvarchar(max))
				End as UserName
		FROM tblLog l 
			Left outer Join tblUsers u on (l.User_Id = u.Id),
			tblSettings s 
		WHERE l.Case_Id = @CaseId and s.Customer_Id = @CurrentCustomerId
		ORDER BY l.Id desc;

		OPEN logData_Cursor

		FETCH NEXT FROM logData_Cursor 
		INTO @Log_Id, @Log_LogDate, @Log_Text_External,
				@Log_Text_Internal, @Log_UserName;			
		

	Set @ExternalFieldName  = '';
	Select @ExternalFieldName = FieldName, @ExternalFieldCaption = FieldCaption From @AvailableFields where FieldName = 'tblLog.Text_External';
	Set @ExternalFieldCaption = Isnull(@ExternalFieldCaption, dbo.GetDefCaseFieldCaption('tbllog_text_external', @LanguageId)) 

	Set @InternalFieldName  = '';
	Select @InternalFieldName = FieldName, @InternalFieldCaption = FieldCaption From @AvailableFields where FieldName = 'tblLog.Text_Internal';	      	 
	Set @InternalFieldCaption = Isnull(@InternalFieldCaption, dbo.GetDefCaseFieldCaption('tbllog_text_internal', @LanguageId)) 
		 		
	WHILE @@FETCH_STATUS = 0
	BEGIN	 
		set @Log_Value = CONVERT(VARCHAR(10), @Log_LogDate, 120)  + '  ' + @Log_UserName;	  
		Insert into @ResultSet (Id, FieldName, FieldCaption, FieldValue, InOrder, LineType) 
				values(@FieldId, 'LogNote', '', @Log_Value, @InOrder, 'L')	  	 

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

		FETCH NEXT FROM logData_Cursor 
		INTO @Log_Id, @Log_LogDate, @Log_Text_External,
			@Log_Text_Internal, @Log_UserName;

	End; /* While Log_Cursor */ 
	
	CLOSE logData_Cursor;
	DEALLOCATE logData_Cursor;

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


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.22'
