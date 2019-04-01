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
                        id: "serviceRequestDetailsTab",
                        name: "Service Request Details",
                        sections: [
                            {
                                id: "coWorkerInformationSec",
                                name: "Co-worker information",                                
                                multiSectionAction: {
                                    allowMultipleSections : true,
                                    actionName: "add"
                                },                                    
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
									{
										id: "usersDropDown1",
										type: "dropdown",
										label: "Select User",
										dataSource: [{value: "value1", text: "Text 1"}, {value: "value2", text: "Text 2"}, {value: "value3", text: "Text 3"}],
									},
                                ]
                            }
                        ]
                    },

                    {
                        id: "reviewTab",
                        name: "Review",
                        sections: [
                            {
                                type:"review",
                                name: "Co-worker information (Review)",                                
                                reviewSectionId: "tabs.serviceRequestDetailsTab.sections.coWorkerInformationSec",                                                                
                                controls: [                        
                                    {
                                        reviewControlId: "firstName",
                                        valueBinding: function(m, log) {
                                            return ["", this.value];
                                        }
                                    },
                                    {
                                        reviewControlId: "lastName",                                        
										valueBinding: function(m, log) {
                                            return ["", this.value];
                                        }
                                    },
									{		
                                        reviewControlId: "usersDropDown1",                                        
										valueBinding: function(m, log) {
                                            return [this.value, this.secondaryValue];
                                        }
                                    }
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