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
        name: "IFrameTest",
        localization: {
        dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
            decimalSeparator: ","
    },
    tabs: [
        {
            id: "tab1",
            name: "Tab1 ",
            sections: [
                {
                    id: "section1",
                    name: "Section 1",
                    controls: [
                        {
                            id: "txt",
                            type: "textbox",
                            label: "Postal example",
							validators: {
								onSave: [{
									type: "pattern",
									value: "[1-9][0-9]{3}\s[a-zA-Z]{2}",
									message: "Wrong format (NNNN LL)"
								}]								
							}
                        }]
                }]            
        }]
}'
           ,'Test'
           ,GETDATE()
           ,'Test'
           ,GETDATE()
           ,'Test')
GO
SET IDENTITY_INSERT [dbo].[ExtendedCaseForms] OFF

EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 