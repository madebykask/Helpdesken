use dhHelpdesk_IKEA_BSCHR
GO

Declare  @CustomerId int
Declare @CustomerName nvarchar(200)

select @CustomerId = Id, @CustomerName = Name from tblCustomer where CustomerGuid = '4DE1C499-4B53-4F4D-A02C-16CDF97E017B'

print Ltrim(@CustomerId) + ' : ' + @CustomerName

Declare @UserId int = 2

Declare @CaseTemplate_Id int


--CaseDocumentTemplate
Declare @CaseDocumentTemplate_LanguageId int = 2
select @CaseDocumentTemplate_LanguageId = Id from tblLanguage where LanguageId = 'EN'
Declare @CaseDocumentTemplate_PageNumbersUse bit = 1
Declare @CaseDocumentTemplate_MarginTop int = 30
Declare @CaseDocumentTemplate_FooterHeight int = 60
Declare @CaseDocumentTemplate_HeaderHeight int = 60

Declare @CaseDocumentTemplateGUID uniqueidentifier


--CaseDocument
Declare @CaseDocumentId int
Declare @CaseDocumentGUID uniqueidentifier
Declare @CaseDocumentName nvarchar(100)

-- CaseDocumentCondition
Declare @CaseDocumentConditionGUID uniqueidentifier
Declare @CaseDocumentCondition_Values nvarchar(max)
Declare @CaseDocumentCondition_Description nvarchar(100)
Declare @CaseDocumentCondition_Property_Name nvarchar(500)


--CaseDocumentText
Declare @CaseDocumentTextConditionGUID uniqueidentifier
Declare @CaseDocumentTextId int = 0 
Declare @Property_Name nvarchar(500) 
Declare @Operator nvarchar(50) 
Declare @Values nvarchar(50) 



--select newid()
set @CaseDocumentTemplateGUID = '5556A10B-BB8D-444A-95F9-6A3A3B1F1589'

if (@CustomerId is not null and @CustomerName is not null and @CaseDocumentTemplate_LanguageId is not null )
begin

	if not exists(select * from tblCaseDocumentTemplate where CaseDocumentTemplateGUID = @CaseDocumentTemplateGUID)
	begin
		INSERT INTO [dbo].[tblCaseDocumentTemplate]
				   (
				   [Name]
				   ,[MarginTop]
				   ,[PageNumbersUse]
				   ,[FooterHeight]
				   ,[HeaderHeight]
				   ,CaseDocumentTemplateGUID)
			 VALUES
				   (
				   @CustomerName
				   ,@CaseDocumentTemplate_MarginTop
				   ,@CaseDocumentTemplate_PageNumbersUse
				   ,@CaseDocumentTemplate_FooterHeight
				   ,@CaseDocumentTemplate_HeaderHeight
				   ,@CaseDocumentTemplateGUID )


		set @CaseTemplate_Id = @@identity

	end
	else
	begin

		update tblCaseDocumentTemplate set
		[Name] =  @CustomerName,
		[MarginTop] = @CaseDocumentTemplate_MarginTop,
		[PageNumbersUse] = @CaseDocumentTemplate_PageNumbersUse,
		[FooterHeight] = @CaseDocumentTemplate_FooterHeight,
		[HeaderHeight] = @CaseDocumentTemplate_HeaderHeight
		where CaseDocumentTemplateGUID = @CaseDocumentTemplateGUID

		select @CaseTemplate_Id = Id from tblCaseDocumentTemplate where CaseDocumentTemplateGUID = @CaseDocumentTemplateGUID
	end


begin /* Case Template Start */

if @CaseTemplate_Id is not null
begin


	--CREATE DOCUMENT
	set @CaseDocumentName = 'Contract Clusters - Retail EA'

	/* THIS SHOULD BE SAME IN ALL ENVIRONMENTS */
	set @CaseDocumentGuid = 'E4BF7D03-3360-4758-B3B3-58EEAAFBF701'
	

	if @CaseTemplate_Id is not null 
	begin

		if not exists(select * from tblCaseDocument where CaseDocumentGUID = @CaseDocumentGuid)
		begin
			INSERT INTO [dbo].[tblCaseDocument]
					   (
					   [CaseDocumentGUID]
					   ,[Name]
					   ,[Description]
					   ,[Customer_Id]
					   ,[FileType]
					   ,[CreatedByUser_Id]
					   ,[CaseDocumentTemplate_Id])
				 VALUES
					   (
					   @CaseDocumentGuid
					   ,@CaseDocumentName
					   ,''
					   ,@CustomerId
					   ,'pdf'
					   ,@UserId
					   ,@CaseTemplate_Id)

			set @CaseDocumentId = @@identity

		end
		else
		begin
			update tblCaseDocument set
			[Name] = @CaseDocumentName,
			[Description] = '',
			[Customer_Id] = @CustomerId,
			[FileType] = 'pdf',
			[CreatedByUser_Id] = @UserId,
			[CaseDocumentTemplate_Id] = @CaseTemplate_Id

			where CaseDocumentGUID = @CaseDocumentGuid

		end
		
	end
	else
	begin
		print 'Can''t find CaseTemplate'
	end


	select @CaseDocumentId = Id from tblCaseDocument where [CaseDocumentGUID] = @CaseDocumentGuid

	if (@CaseTemplate_Id is not null and @CaseDocumentId is not null)
	begin
	
			begin /* CaseDocumentCondition ProductArea */

			-- SHOULD BE SAME IN ALL ENVIRONMENTS!!
			--select newid()
			set @CaseDocumentConditionGUID = '92D4FE4F-A1E5-4922-A744-2BE1569D1056'

			--//SET TO PRODUCTAREA GUID
			--Hiring - SHOULD BE SAME IN ALL ENVIRONMENTS
			set @CaseDocumentCondition_Values = '10ED7779-E56E-4C75-B02C-4486D8029DCE'
			set @CaseDocumentCondition_Property_Name = 'case_ProductArea.ProductAreaGUID'
			set @CaseDocumentCondition_Description = 'ProductArea = Hiring '
		
				if not exists(select * from tblCaseDocumentCondition where CaseDocumentConditionGUID = @CaseDocumentConditionGUID)
				begin
					INSERT INTO [dbo].[tblCaseDocumentCondition]
							   ([CaseDocumentConditionGUID]
							   ,[CaseDocument_Id]
							   ,[Property_Name]
							   ,[Values]
							   ,[Description]
							   ,[Status]
							   ,[CreatedByUser_Id]
							   )
						 VALUES
							   (@CaseDocumentConditionGUID
							   ,@CaseDocumentId
							   ,@CaseDocumentCondition_Property_Name
							   ,@CaseDocumentCondition_Values
							   ,@CaseDocumentCondition_Description
							   ,1
							   ,@UserId
							   )

				end
				else
				begin
						update tblCaseDocumentCondition set
						CaseDocument_Id = @CaseDocumentId,
						[Property_Name] = @CaseDocumentCondition_Property_Name,
						[Values] = @CaseDocumentCondition_Values,
						[Description] = @CaseDocumentCondition_Description,
						ChangedDate = getDate(),
						ChangedByUser_Id = @UserId
						where CaseDocumentConditionGUID = @CaseDocumentConditionGUID
				end

			end /* CaseDocumentCondition ProductArea */

			

			begin /* CaseDocumentCondition PayRollCategory */

			-- SHOULD BE SAME IN ALL ENVIRONMENTS!!
			--select newid()
			set @CaseDocumentConditionGUID = 'D1A696B0-E14A-4270-91CB-7B9A56927E02'

			set @CaseDocumentCondition_Values = 'QW'
			set @CaseDocumentCondition_Property_Name = 'extendedcase_tabs.PaymentInformation.sections.STD_S_BasicPayHiring.instances[0].controls.PayrollCategory'
			set @CaseDocumentCondition_Description = 'PayRollCategory = Waged'
		
				if not exists(select * from tblCaseDocumentCondition where CaseDocumentConditionGUID = @CaseDocumentConditionGUID)
				begin
					INSERT INTO [dbo].[tblCaseDocumentCondition]
							   ([CaseDocumentConditionGUID]
							   ,[CaseDocument_Id]
							   ,[Property_Name]
							   ,[Values]
							   ,[Description]
							   ,[Status]
							   ,[CreatedByUser_Id]
							   )
						 VALUES
							   (@CaseDocumentConditionGUID
							   ,@CaseDocumentId
							   ,@CaseDocumentCondition_Property_Name
							   ,@CaseDocumentCondition_Values
							   ,@CaseDocumentCondition_Description
							   ,1
							   ,@UserId
							   )

				end
				else
				begin
						update tblCaseDocumentCondition set
						CaseDocument_Id = @CaseDocumentId,
						[Property_Name] = @CaseDocumentCondition_Property_Name,
						[Values] = @CaseDocumentCondition_Values,
						[Description] = @CaseDocumentCondition_Description,
						ChangedDate = getDate(),
						ChangedByUser_Id = @UserId
						where CaseDocumentConditionGUID = @CaseDocumentConditionGUID
				end

			end /* CaseDocumentCondition PayRollCategory */

				begin /* CaseDocumentCondition Region */

			-- SHOULD BE SAME IN ALL ENVIRONMENTS!!
			--select newid()
			set @CaseDocumentConditionGUID = '4EE7578E-876A-421E-B19C-00713449AB90'

			set @CaseDocumentCondition_Values = '1300'
			set @CaseDocumentCondition_Property_Name = 'case_Region.Code'
			set @CaseDocumentCondition_Description = 'Company Code = 1300'
		
				if not exists(select * from tblCaseDocumentCondition where CaseDocumentConditionGUID = @CaseDocumentConditionGUID)
				begin
					INSERT INTO [dbo].[tblCaseDocumentCondition]
							   ([CaseDocumentConditionGUID]
							   ,[CaseDocument_Id]
							   ,[Property_Name]
							   ,[Values]
							   ,[Description]
							   ,[Status]
							   ,[CreatedByUser_Id]
							   )
						 VALUES
							   (@CaseDocumentConditionGUID
							   ,@CaseDocumentId
							   ,@CaseDocumentCondition_Property_Name
							   ,@CaseDocumentCondition_Values
							   ,@CaseDocumentCondition_Description
							   ,1
							   ,@UserId
							   )

				end
				else
				begin
						update tblCaseDocumentCondition set
						CaseDocument_Id = @CaseDocumentId,
						[Property_Name] = @CaseDocumentCondition_Property_Name,
						[Values] = @CaseDocumentCondition_Values,
						[Description] = @CaseDocumentCondition_Description,
						ChangedDate = getDate(),
						ChangedByUser_Id = @UserId
						where CaseDocumentConditionGUID = @CaseDocumentConditionGUID
				end

			end /* CaseDocumentCondition Region */




	end
	else
	begin
		print 'no insert in CaseDocumentCondition' + ltrim(@CaseDocumentConditionGUID)
	end
	/* PARAGRAPHS */

	/* TODO: vad är generellt, flytta till annat script så vi har det 1 gång. och vad är specifikt för det här kontraktet???? */

	Declare @ParagraphTypeText int = 1
	Declare @ParagraphTypeTableNumeric int = 2
	Declare @ParagraphTypeLogo int = 3
	Declare @ParagraphTypeTableTwoColumns int = 4
	Declare @ParagraphTypeFooter int = 5
	Declare @ParagraphTypeDraft int = 6
	Declare @ParagraphTypeHeader int = 7

	Declare @CaseDocumentParagraph_Id int
	Declare @SortOrder int

	declare @CaseDocumentParagraphGUID uniqueidentifier = null
	declare @CaseDocumentParagraphName nvarchar(100) = ''

	Declare @Text nvarchar(max) = ''
	Declare @Headline nvarchar(200) = ''

begin /* Logo */

	declare @CaseDocumentParagraphGUIDLogo uniqueidentifier = 'EB0434AA-0BBF-4CA8-AB0A-BF853129FB9D'
	declare @CaseDocumentParagraphNameLogo nvarchar(100) = 'Logo'

	

	if not exists(select * from tblCaseDocumentParagraph where [Name] = @CaseDocumentParagraphNameLogo or CaseDocumentParagraphGUID =  @CaseDocumentParagraphGUIDLogo)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphNameLogo
				   ,''
				   ,@ParagraphTypeLogo
				   ,@CaseDocumentParagraphGUIDLogo
				   )

	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUIDLogo


	set @SortOrder = 1
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

	
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	else
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Logo */



begin /* Paragraph - AU - Address */
	
	set @CaseDocumentParagraphGUID = '3E55AA5C-B241-4C01-9DB3-837B07118BF7'
	set @CaseDocumentParagraphName = 'AU - Address'


	Declare @CaseDocumentTextGUID uniqueidentifier = null
	Declare @CaseDocumentTextName nvarchar(50) = ''
	Declare @TextSortOrder int = 0


	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Address section, used in all contracts AU'
				   ,@ParagraphTypeText
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeText

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

		begin /* Text - CompanyText */
		
		set @CaseDocumentTextGUID = 'DA61FAA7-27D1-4E88-BC7B-C555F8483FE3'
		set @CaseDocumentTextName  = 'Company Text'
		set @TextSortOrder = 0
	
		set @Text = 
		'<p style="text-align:center;">IKEA Pty Limited ABN 84 006 270 757</p>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* Text - CompanyText */


	begin /* Text - Address Text */

		set @TextSortOrder = 2


		set @CaseDocumentTextGUID  = 'EAD6D4A7-7A9A-4F6F-84D5-6BA4C57CEB71'
		set @CaseDocumentTextName = 'Address Text'
	
		set @Text = 
		'<p><Todays Date - Long></p>
		<p><Co-worker First Name> <Co-worker Last Name></p>
		<p><Address Line 1><br />
		<Address Line 2> <State> <Postal Code><br />
		<Address Line 3><br />
		<Co-worker GlobalView ID>
		<br /><br />
		Dear <Co-worker First Name></p>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* Text - Address Text */

	--CONNNECT PARAGRAPH WITH DOCUMENT

	set @SortOrder = 2
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

		
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Paragraph - AU - Address */





begin  /* Paragraph - REST */
	
	--select newid()
	set @CaseDocumentParagraphGUID = 'AC6891B8-18CF-45F3-A4C0-CC27294C6DF2'
	set @CaseDocumentParagraphName = 'Contract Clusters - DC EA - BODY'




	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Contract Clusters - DC EA'
				   ,@ParagraphTypeText
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeText

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

		begin /* 10a */
		
		set @CaseDocumentTextGUID = '8E52C1BE-9CB5-43DF-87BF-E4EA8B9A91E2'
		set @CaseDocumentTextName  = '10a'
		set @TextSortOrder = 0
	
		set @Text = 
		'IKEA Pty Ltd (IKEA) is pleased to present you with a contract of employment under the terms and conditions of the IKEA Enterprise Agreement 2017.'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

		begin /* 10a CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'C547488F-E403-471F-A1F7-A36948018CB9'
			
			set @Property_Name= 'extendedcase_ContractEndDate'
			
			set @Operator= 'IsEmpty'
			
			set @Values = ''

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)

				

			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end




		end /* 10a CONDITIONS */

	end /* 10a */


	begin /* 10b */

		set @TextSortOrder = 0

		
		set @CaseDocumentTextGUID  = 'E8B2F985-D317-4E06-9934-7593AC6C1A1E'
		set @CaseDocumentTextName = '10b'
	
		set @Text = 
		'IKEA Pty Ltd (IKEA) is pleased to present you with a temporary contract of employment under the terms and conditions of the IKEA Enterprise Agreement 2017.'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end


			begin /* 10b CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = '6B682E31-49E6-4A85-9251-69F279ECBFB1'
			
			set @Property_Name= 'extendedcase_ContractEndDate'
			set @Operator= 'HasValue'
			set @Values = ''

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)

				

			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end




		end /* 10b CONDITIONS */


	end /* 10b */


	--CONNNECT PARAGRAPH WITH DOCUMENT

	set @SortOrder = 3
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

		
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Paragraph - REST */

begin  /* Paragraph - TABLE */
	
	--select newid()
	set @CaseDocumentParagraphGUID = '9138949C-F253-4803-9E0E-4643811D5DEA'
	set @CaseDocumentParagraphName = 'Contract Clusters - DC EA - TABLE'

	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Contract Clusters - DC EA - TABLE'
				   ,@ParagraphTypeTableTwoColumns
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeTableTwoColumns

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

	begin /* 11 */

		set @TextSortOrder = 0
		
		set @CaseDocumentTextGUID  = '350AE198-CABA-47C9-BA6D-10441E0DD3D7'
		set @CaseDocumentTextName = '11'
	
		set @Headline = 'Classification:'
		set @Text = '<Position Title (Local Job Name)>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* 11 */


		begin /* 12 */

		set @TextSortOrder = 0
		
		set @CaseDocumentTextGUID  = '53CF156C-7029-4D46-8C4B-0BF44A4FDE82'
		set @CaseDocumentTextName = '12'
	
		set @Headline = 'Location:'
		set @Text = '<Business Unit>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* 12 */

	
		begin /* 13a */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = 'C1122EE9-4EA2-4365-B091-F2AC6C8915ED'
		set @CaseDocumentTextName = '13a'
	
		set @Headline = 'Effective date:'
		set @Text = '<Contract Start Date>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end


			begin /* 13a CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'CF362805-DA68-4548-A5F5-8A9E47577436'
			
			set @Property_Name= 'extendedcase_ContractEndDate'
			
			set @Operator= 'IsEmpty'
			
			set @Values = ''

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)

				

			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end




		end /* 13a CONDITIONS */



	end /* 13a */



		begin /* 13b */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = '445B4588-CD93-4F51-BC70-C2B2FD1B3810'
		set @CaseDocumentTextName = '13b'
	
		set @Headline = 'Temporary Contract Period:'
		set @Text = 'from <Contract Start Date>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end


			begin /* 13b CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'DADC6E18-B2F2-4450-A61E-E2E29210CE32'
			
			set @Property_Name= 'extendedcase_ContractEndDate'
			
			set @Operator= 'HasValue'
			
			set @Values = ''

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end




		end /* 13b CONDITIONS */



	end /* 13b */




		begin /* 14a */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = '488A0CCB-F937-428D-9271-0F9947E34A10'
		set @CaseDocumentTextName = '14a'
	
		set @Headline = 'Employment type:'
		set @Text = 'Full Time'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

			begin /* 14a CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'EC6ED1A1-0DB5-40EE-A71D-B72CE9E9405E'
			
			set @Property_Name= 'extendedcase_ContractedHours'
			
			set @Operator= 'Equal'
			
			set @Values = '76'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end




		end /* 14a CONDITIONS */

	end /* 14a */

	begin /* 14b */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = 'D4FB935F-8AD2-4F73-BD10-A616DAD2B46F'
		set @CaseDocumentTextName = '14b'
	
		set @Headline = 'Employment type:'
		set @Text = 'Part Time'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

			begin /* 14b CONDITIONS NR 1 */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'A9786F2D-441E-4EB9-909C-00C619838ECC'
			
			set @Property_Name= 'extendedcase_ContractedHours'
			
			set @Operator= 'LessThan'
			
			set @Values = '76'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end


		end  /* 14b CONDITIONS NR 1 */

			begin /* 14b CONDITIONS NR 2 */

			--select newid()
			set @CaseDocumentTextConditionGUID = '08E59549-8574-4DDD-AE75-724486A49CE3'
			
			set @Property_Name= 'extendedcase_ContractedHours'
			
			set @Operator= 'LargerThan'
			
			set @Values = '0'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end


		end  /* 14b CONDITIONS NR 2 */

	end /* 14b */

	begin /* 14c */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = '00B85E9F-FAC3-4FF6-AD75-01326E096231'
		set @CaseDocumentTextName = '14c'
	
		set @Headline = 'Employment type:'
		set @Text = 'Casual'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

		begin /* 14c CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'FDADC78A-E395-442B-9BAD-355AE56AEBC3'
			
			set @Property_Name= 'extendedcase_ContractedHours'
			
			set @Operator= 'Equal'
			
			set @Values = '0'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end


		end  /* 14c CONDITIONS */

	end /* 14c */


		begin /* 15a */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = '8FF33EAD-BEF3-469E-ABC1-37B5A5B9D681'
		set @CaseDocumentTextName = '15a'
	
		set @Headline = 'Contracted Hours per Fortnight:'
		set @Text = '<Contracted Hours>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

		begin /* 15a CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'C7E9D22F-A65B-4E8C-9A70-0FB0E2F22F58'
			
			set @Property_Name= 'extendedcase_ContractedHours'
			
			set @Operator= 'LargerThan'
			
			set @Values = '0'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end


		end  /* 15a CONDITIONS */


	end /* 15a */

	begin /* 15b */

		set @TextSortOrder = 0
		--select newid()
		set @CaseDocumentTextGUID  = 'B64FE969-F1DB-46E1-A893-0CDA60212619'
		set @CaseDocumentTextName = '15b'
	
		set @Headline = ''
		set @Text = ''
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,@Headline
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[Headline] = @Headline,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

			begin /* 15b CONDITIONS */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'A8F95C94-3900-4B92-B846-506B5E105DFE'
			
			set @Property_Name= 'extendedcase_ContractedHours'
			
			set @Operator= 'Equal'
			
			set @Values = '0'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)
			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end


		end  /* 15b CONDITIONS */


	end /* 15b */

	--CONNNECT PARAGRAPH WITH DOCUMENT

	set @SortOrder = 4
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

		
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Paragraph - TABLE */


begin  /* Paragraph - SIGNATURE */
	
	--select newid()
	set @CaseDocumentParagraphGUID = 'A933C122-8E38-43CD-B857-1249C9AD463C'
	set @CaseDocumentParagraphName = 'Contract Clusters - DC EA - SIGNATURE'

	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Contract Clusters - DC EA - SIGNATURE'
				   ,@ParagraphTypeText
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeText

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

		begin /* 16 */
		--select newId()
		set @CaseDocumentTextGUID = '696409BA-C2D0-4BA2-809D-9ED7E9D0141C'
		set @CaseDocumentTextName  = '16'
		set @TextSortOrder = 0
	
		set @Text = 
		'Your terms and conditions of employment will be as per the IKEA Enterprise Agreement 2017, the IKEA Group Code of Conduct and IKEA policies and procedures, as amended from time to time.  You can access this information via ‘ico-worker.com’ <i>(the IKEA co-worker website)</i> or ‘IKEA Inside’ <i>(the IKEA intranet)</i>.'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* 16 */


		begin /* 17,18,19 */
		--select newId()
		set @CaseDocumentTextGUID = '2E936F7F-62C2-4A60-BDDB-66852F6FD273'
		set @CaseDocumentTextName  = '17,18,19'
		set @TextSortOrder = 0
	
		set @Text = 
		'<p><Reports To Line Manager><br />
		<Position Title (Local Job Name) of Reports To Line Manager><br />
		<Business Unit></p>
		<hr style="height:2px;border:none;color:#000;background-color:#000;" />
		'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* 17,18,19 */

		begin /* 20 */
		--select newId()
		set @CaseDocumentTextGUID = '6D8BA196-5157-4765-8933-05F23FA6775E'
		set @CaseDocumentTextName  = '20'
		set @TextSortOrder = 0
	
		set @Text = 
		'<p style="text-align:center;"><strong>Acknowledgement</strong></p>
		<p>I accept the terms and conditions of employment as detailed above.</p>
		<p>Signed:  _____________________________        Date:  _______________</p>
		<p>Co-worker Name: <Co-worker First Name> <Co-worker Last Name></p>
		'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* 20 */

	--CONNNECT PARAGRAPH WITH DOCUMENT

	set @SortOrder = 5
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

		
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Paragraph - SIGNATURE */




begin /* Paragraph - FOOTER*/
	
	
	set @CaseDocumentParagraphGUID = 'D43619B6-BE1C-4DEF-AF32-460CF8D38F63'
	set @CaseDocumentParagraphName = 'AU - FOOTER'

	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Footer section, used in all contracts AU'
				   ,@ParagraphTypeFooter
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeFooter

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

		begin /* Text - Footer */

		set @CaseDocumentTextGUID = 'D904B065-7FE0-4D95-85F3-04F4056D450F'
		set @CaseDocumentTextName  = 'Text - Footer'
		set @TextSortOrder = 1
	

		set @Text = 
		'<!DOCTYPE html><html><head></head><body style="text-align:center !important; width:100%;"><p style="color:#ccc; font-size:16px; font-family:Verdana !important;"><strong>Confidential</strong><br />Contract with <Co-worker First Name> <Co-worker Last Name> dated <Todays Date - Short></p></body></html>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end

	end /* Text - CompanyText */


	
	--CONNNECT PARAGRAPH WITH DOCUMENT

	set @SortOrder = 0
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

		
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Paragraph - FOOTER */


begin /* Paragraph - DRAFT*/
	
	--select newid()
	set @CaseDocumentParagraphGUID = '51220147-E756-492E-88A1-C1671BDE6AA5'
	set @CaseDocumentParagraphName = 'STD - DRAFT'

	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
	begin

		INSERT INTO [dbo].[tblCaseDocumentParagraph]
				   ([Name]
				   ,[Description]
				   ,[ParagraphType]
				   ,CaseDocumentParagraphGUID)
			 VALUES
				   (@CaseDocumentParagraphName
				   ,'Draft section'
				   ,@ParagraphTypeDraft
				   ,@CaseDocumentParagraphGUID
				   )

	end
	else
	begin
		
		update [tblCaseDocumentParagraph] set
		[Name] = @CaseDocumentParagraphName,
		[ParagraphType] = @ParagraphTypeDraft

		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
	end

	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

		begin /* Text - Draft */
		--select newid()
		set @CaseDocumentTextGUID = 'E448A518-9FFB-46EA-B4B7-FA824D24A05E'
		set @CaseDocumentTextName  = 'Text - Draft'
		set @TextSortOrder = 0
	
		set @Text = 
		'<!DOCTYPE html><html><head><style>body{-ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";filter: alpha(opacity=50);-moz-opacity: 0.5;-khtml-opacity: 0.5;opacity: 0.5;}</style></head><body class="draft"><br /><br /><br /><br /><br /><br /><p style="color:#ccc; font-size:150px; font-family:Verdana;">Draft</p></body></html>'
	
		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
		begin

			INSERT INTO [dbo].[tblCaseDocumentText]
					   ([CaseDocumentParagraph_Id]
					   ,[Name]
					   ,[Description]
					   ,[Text]
					   ,[Headline]
					   ,[SortOrder]
					   ,CaseDocumentTextGUID)
				 VALUES
					   (@CaseDocumentParagraph_Id
					   ,@CaseDocumentTextName
					   ,''
					   ,@Text
					   ,''
					   ,@TextSortOrder
					   ,@CaseDocumentTextGUID)
		end
		else
		begin

			update [tblCaseDocumentText] set
			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
			[Name] = @CaseDocumentTextName,
			[Text] = @Text,
			[SortOrder] = @TextSortOrder
			where CaseDocumentTextGUID = @CaseDocumentTextGUID
		end


			begin /* DRAFT CONDITIONS NR 1 */

			--select newid()
			set @CaseDocumentTextConditionGUID = '5D9E237D-E3BC-444D-8A5A-45ECF1E5DC20'
			
			set @Property_Name= 'case_StateSecondary.StateSecondaryId'
			
			set @Operator= 'LargerThanOrEqual'
			
			set @Values = '20'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)

				

			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end
		end /* DRAFT CONDITIONS NR 1 */


			begin /* DRAFT CONDITIONS NR 2 */

			--select newid()
			set @CaseDocumentTextConditionGUID = 'CF263AE3-D541-4C4E-974F-047EDCBC5078'
			
			set @Property_Name= 'case_StateSecondary.StateSecondaryId'
			
			set @Operator= 'LessThan'
			
			set @Values = '40'

			-- GET FROM ABOVE 
			select @CaseDocumentTextId = Id from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID
			
			if not exists(select * from tblCaseDocumentTextCondition where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID)
			begin
				
				Insert into tblCaseDocumentTextCondition
				(CaseDocumentTextConditionGUID, 
				CaseDocumentText_Id,
				Property_Name,
				[Operator],
				[Values],
				[Status])
				VALUES
				(
				@CaseDocumentTextConditionGUID,
				@CaseDocumentTextId,
				@Property_Name,
				@Operator,
				@Values,
				1
				)

				

			end
			else
			begin

				update tblCaseDocumentTextCondition 
				set CaseDocumentText_Id = @CaseDocumentTextId,
				Property_Name = @Property_Name,
				Operator = @Operator,
				[Values]= @Values
				where CaseDocumentTextConditionGUID = @CaseDocumentTextConditionGUID

			end
		end /* DRAFT CONDITIONS NR 2 */


	end  /* Text - Draft */


	
	--CONNNECT PARAGRAPH WITH DOCUMENT

	set @SortOrder = 0
	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
	begin

		
		insert into tblCaseDocument_CaseDocumentParagraph
		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

	end
	begin
		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
	end

end /* Paragraph - DRAFT */





--begin /* Paragraph - HEADER*/
	
--	select newid()
--	set @CaseDocumentParagraphGUID = 'F30A9584-EC7C-40D9-90D1-29EC6BF95EE2'
--	set @CaseDocumentParagraphName = 'AU - HEADER'

--	if not exists(select * from tblCaseDocumentParagraph where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID)
--	begin

--		INSERT INTO [dbo].[tblCaseDocumentParagraph]
--				   ([Name]
--				   ,[Description]
--				   ,[ParagraphType]
--				   ,CaseDocumentParagraphGUID)
--			 VALUES
--				   (@CaseDocumentParagraphName
--				   ,'test'
--				   ,@ParagraphTypeHeader
--				   ,@CaseDocumentParagraphGUID
--				   )

--	end
--	else
--	begin
		
--		update [tblCaseDocumentParagraph] set
--		[Name] = @CaseDocumentParagraphName,
--		[ParagraphType] = @ParagraphTypeHeader

--		where CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID
--	end

--	select @CaseDocumentParagraph_Id = Id from tblCaseDocumentParagraph where  CaseDocumentParagraphGUID = @CaseDocumentParagraphGUID

--		begin /* Text - Footer */
--		select newid()
--		set @CaseDocumentTextGUID = 'BD12B3E6-705D-4823-B433-E369FB5076D6'
--		set @CaseDocumentTextName  = 'Text - Header'
--		set @TextSortOrder = 1
	

--		set @Text = 
--		'<!DOCTYPE html><html><head></head><body style="text-align:center !important; width:100%;"><p style="color:#000; font-size:16px; font-family:Verdana !important;">&p; of &P;</p></body></html>'
	
--		if not exists(select * from tblCaseDocumentText where CaseDocumentTextGUID = @CaseDocumentTextGUID)
--		begin

--			INSERT INTO [dbo].[tblCaseDocumentText]
--					   ([CaseDocumentParagraph_Id]
--					   ,[Name]
--					   ,[Description]
--					   ,[Text]
--					   ,[Headline]
--					   ,[SortOrder]
--					   ,CaseDocumentTextGUID)
--				 VALUES
--					   (@CaseDocumentParagraph_Id
--					   ,@CaseDocumentTextName
--					   ,''
--					   ,@Text
--					   ,''
--					   ,@TextSortOrder
--					   ,@CaseDocumentTextGUID)
--		end
--		else
--		begin

--			update [tblCaseDocumentText] set
--			[CaseDocumentParagraph_Id] = @CaseDocumentParagraph_Id,
--			[Name] = @CaseDocumentTextName,
--			[Text] = @Text,
--			[SortOrder] = @TextSortOrder
--			where CaseDocumentTextGUID = @CaseDocumentTextGUID
--		end

--	end /* Text - CompanyText */


	
--	--CONNNECT PARAGRAPH WITH DOCUMENT

--	set @SortOrder = 0
--	if not exists(select * from tblCaseDocument_CaseDocumentParagraph where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id)
--	begin

		
--		insert into tblCaseDocument_CaseDocumentParagraph
--		(CaseDocument_id, CaseDocumentParagraph_Id, SortOrder)
--		VALUES (@CaseDocumentId, @CaseDocumentParagraph_Id,@SortOrder)

--	end
--	begin
--		update tblCaseDocument_CaseDocumentParagraph set SortOrder = @SortOrder where CaseDocument_id = @CaseDocumentId and CaseDocumentParagraph_Id = @CaseDocumentParagraph_Id 
--	end

--end /* Paragraph - HEADER */




end -- check for customerid and customername


end -- check for CaseTemplateId

end /* Case Template Start */

	
