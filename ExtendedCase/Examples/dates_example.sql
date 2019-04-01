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
            dateFormat: "Incorrect date format",
			dateYearFormat: "Year format incorrect."
    },
    tabs: [
        {
            id: "dateTab",
            name: "Date Tab",
            sections: [
                {
                    id: "section1",
                    name: "Section 1",
                    controls: [
                        {
                            id: "date11",
                            type: "date",
                            label: "Date Year",
							mode: "year",							
                        },
						{
                            id: "date12",
                            type: "date",
                            label: "Date",
                        },
						{
                            id: "date13",
                            type: "date",
                            label: "Date with warning",
							warningBinding: function(m, log) {
								var today = moment().startOf("day");
								var date = moment(this.value, m.localization.dateFormat, true);
								if(date.isValid() && today.isAfter(date)) {
									return "The date is in the past, do you want to continue?";
									}
							},
                        },
						]
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