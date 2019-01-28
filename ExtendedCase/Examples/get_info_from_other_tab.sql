EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO
DELETE FROM [dbo].[ExtendedCaseForms] WHERE id=21

SET IDENTITY_INSERT [dbo].[ExtendedCaseForms] ON
INSERT INTO [dbo].[ExtendedCaseForms]
           ([Id]
		   ,[MetaData]
           ,[Description]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           (21, '{
    id: 21,
    name: "Test",
    localization: {
        dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
        decimalSeparator: ","
    },
    tabs: [
        {
            id: "serviceRequestDetails",
            name: "Service Request Details",
            sections: [
                {
                    id: "info",
                    name: "Co-worker information",
                    controls: [                        
                        {
                            id: "firstName",
							type: "textbox",
							label: "Co-worker First Name"
                        },
                        {
                            id: "lastName",
							type: "textbox",
							label: "Co-worker Last Name"
                        },
                    ]
                }
            ]
        },
		{
		    id: "personalInfo",
            name: "Personal Info",
            sections: [
				{
					id: "mainBankDetails",
					name: "Main Bank Details",
					controls: [
                        {
                            id: "payee1",
							type: "textbox",
							label: "Payee - change always",
							valueBinding: function(m, log) {	
								return m.tabs.serviceRequestDetails.sections.info.controls.firstName.value + " "
									 + m.tabs.serviceRequestDetails.sections.info.controls.lastName.value;
							}
                        },
                        {
                            id: "payee2",
							type: "textbox",
							label: "Payee - change only if Firstname and Lastname change",
							valueBinding: function(m, log) {
								var isChanged = false;
								if(log.logs.length > 0)	{
									isChanged = log.logs.find(function (d) {
											return d.id === "tabs.serviceRequestDetails.sections.info.controls.firstName" ||
												d.id === "tabs.serviceRequestDetails.sections.info.controls.lastName";
										}) !== null; 
								} 
								if(isChanged) {
										return m.tabs.serviceRequestDetails.sections.info.controls.firstName.value + " "
											 + m.tabs.serviceRequestDetails.sections.info.controls.lastName.value;
									 }
							}
                        },
						{
                            id: "payee3",
							type: "textbox",
							label: "Payee - change only if field not changed by user",
							valueBinding: function(m, log) {
								if(this.pristine || this.value == "") { // if control is not changed by user
										return m.tabs.serviceRequestDetails.sections.info.controls.firstName.value + " "
											 + m.tabs.serviceRequestDetails.sections.info.controls.lastName.value;
									 }								
							}
                        },
					]					
				}
			]
		}
    ]
}'
           ,'Test'
           ,GETDATE()
           ,'Test'
           ,GETDATE()
           ,'Test')
GO
SET IDENTITY_INSERT [dbo].[ExtendedCaseForms] OFF

EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 