DECLARE @metaData nvarchar(MAX) = 
'{
	id: 1,
	name: "@Translation.Name",
    dataSources:	
	[
		//static data
		{
			type: "static",
			id : "SomeDataSource1",            
			data: [{ prop1: "value1" }, { prop2: "value2"}]
		},       
		
		//avaialble users
		{
			type: "query",			
            id: "getAvaialableUsers"			
        },

		//customers for selected user
		{
			type: "query",			
            id: "customersByUserQuery",
			parameters: [{ name: "userId", field: "tabs.tab1.sections.sec1.controls.usersDropDown1"}]            
        },

		// departments for selected customer
		{
			type: "query",			
			id: "customerDepartmentsQueryDs",
			parameters: [{ name: "customerId", field: "tabs.tab1.sections.sec1.controls.customerDropDown1"}]
		},
    ],
	tabs: [{
		id: "tab1",
		name: "Tab 1",
		sections: [{
			id: "sec1",
			name: "Section 1",
			controls: [

			//simple
			{
				id: "id1",
				type: "textbox",
				label: "TextBox 1"
			},

			//users
			{
				id: "usersDropDown1",
				type: "dropdown",
				label: "Select User",
				dataSource: {
                    type: "custom",
                    id: "getAvaialableUsers",
					valueField: "Id",
					textField: "FirstName"
                }
			},
			
			//customers for selected user
			{
				id: "customerDropDown1",
				type: "dropdown",
				label: "Select Customer",
				dataSource: {
                    type: "custom",
                    id: "customersByUserQuery",
					valueField: "Id",
					textField: "Name"
                }
			},

			//select email of the selected customer from the loaded datasource
			{
				id: "txtSelectedCustomerEmail",
				type: "textbox",
				label: "Customer Email:",
				valueBinding: function(m, log){					
					var ds = m.dataSources.customersByUserQuery;
					var customersDropDown = m.tabs.tab1.sections.sec1.controls.customerDropDown1;
					if (ds && customersDropDown && customersDropDown.value !== undefined){										
						var selectedCustomer = ds.find(function(el){
							if (el.Id === customersDropDown.value)
								return el;
						});	
													
						if (selectedCustomer && selectedCustomer.HelpdeskEmail !== undefined){
							return selectedCustomer.HelpdeskEmail;
						}	
					}
					else
						return "";										
				}
			}] //controls
		}] //sections
	}]//tabs
}'

SET IDENTITY_INSERT [ExtendedCaseForms] ON;

DELETE FROM [dbo].[ExtendedCaseForms] WHERE Id = 2

INSERT INTO [dbo].[ExtendedCaseForms]
           (Id
		   ,[MetaData]
           ,[Description]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           (2
		   ,@metaData
           ,'load save form data test template'
           ,GETDATE()
           ,'test'
           ,GETDATE()
           ,'test')

SET IDENTITY_INSERT [ExtendedCaseForms] OFF;
GO

/*=======================================================================
  == ExtendedCaseData & ExtendedCaseValues test data
  =======================================================================*/
DECLARE @caseDataId int,
		@uniqueId uniqueidentifier

SET @uniqueId = newid()


SET @caseDataId = ISNULL((SELECT TOP 1 Id from [dbo].[ExtendedCaseData] where [ExtendedCaseFormId] = 2),0)

IF (@caseDataId > 0)
BEGIN 
	DELETE FROM dbo.[ExtendedCaseValues] where ExtendedCaseDataId = @caseDataId
	DELETE FROM [dbo].[ExtendedCaseData] where Id = @caseDataId
END

--insert into ExtendedCaseData
INSERT INTO [dbo].[ExtendedCaseData](ExtendedCaseGuid, ExtendedCaseFormId, CreatedOn, CreatedBy)
VALUES (@uniqueId, 2, GETDATE(), 'me')

SET @caseDataId = scope_identity()

INSERT INTO [dbo].[ExtendedCaseValues] (ExtendedCaseDataId, FieldId, Value, SecondaryValue)
VALUES(@caseDataId, 'tabs.tab1.sections.sec1.fields.id1', 'hello', null),
	  (@caseDataId, 'tabs.tab1.sections.sec1.fields.usersDropDown1', '8', 'Per')

	  
--[dbo].[ExtendedCaseValues]