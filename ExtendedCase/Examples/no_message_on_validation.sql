EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO
DELETE FROM [dbo].[ExtendedCaseForms] WHERE id=23

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
           (23, '{
    id: 23,
        name: "IFrameTest",
        localization: {
        dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
            decimalSeparator: ","
    },
    validatorsMessages: {
		required: "Required.",
		noMessage: ""
    },
    tabs: [
        {
            id: "tab1",
            name: "Tab 1",
            sections: [
                {
                    id: "section1",
                    name: "Section 1",
                    controls: [
                        {
                            id: "txt1",
                            type: "textbox",
                            label: "Empty message",
							validators: {
								onSave: [
									{
										type: "required",
										message: ""
									}
								]
							}
                        },
                        {
                            id: "txt2",
                            type: "textbox",
                            label: "Empty messageName",
							validators: {
								onSave: [
									{
										type: "required",
										messageName: "noMessage"
									}
								]
							}
                        },
                        {
                            id: "txt3",
                            type: "textbox",
                            label: "Standard message",
							validators: {
								onSave: [
									{
										type: "required"
									}
								]
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