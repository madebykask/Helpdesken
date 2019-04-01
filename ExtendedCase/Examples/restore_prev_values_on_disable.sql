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
                tabs: [{
                        id: "serviceRequestDetails",
                        name: "Service Request Details",
                        sections: [
                            {
                                id: "info",
                                name: "Co-worker information",
                                enableAction: {               
                                    initialState: true,
                                    label: "Edit"
                                },  
                                multiSectionAction: {
                                    allowMultipleSections : true,
                                    actionName: "add"
                                },    
                                disabledStateAction: "RestorePrev", // Clear, Keep, RestorePrev    
                                controls: [                        
                                    {
                                        id: "firstName",
					               type: "textbox",
					               label: "First Name"
                                    },
                                    {
                                        id: "lastName",
					               type: "textbox",
					               label: "Last Name"
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