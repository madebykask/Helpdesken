EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO
DELETE FROM [dbo].[ExtendedCaseForms] WHERE id=22

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
           (22, '{
    id: 22,
    name: "Voluntary Deductions - Standard",
    localization: {
        dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
        decimalSeparator: ","
    },
    tabs: [
        {
            id: "PaymentInformation",
            name: "Payment Information",
            sections: [
                {
                    id: "Deductions",
                    name: "Deductions",
                    controls: [                        
                        {
                            id: "DeductionsAmountUnits",
							type: "radio",
							label: "Amount/Units",
                            dataSource: [{value: "Amount", text: "Amount"}, {value: "Unit", text: "Unit"}]
                        },
                        {
							id: "DeductionsAmount_Amount",
							type: "amount",
							label: "",
                            addonText: "ˆ",
                            hiddenBinding: function(m, log){								
                                return m.tabs.PaymentInformation.sections.Deductions.controls.DeductionsAmountUnits.value !== "Amount";
                            },
							valueBinding: function(m) {
								// remove value if control is hidden
								if(this.hidden) return "";
							}
                        },
                        {
							id: "DeductionsAmount_Unit",
							type: "unit",
							label: "",
                            hiddenBinding: function(m, log){
                                return m.tabs.PaymentInformation.sections.Deductions.controls.DeductionsAmountUnits.value !== "Unit";
                            },
							valueBinding: function(m) {
								// remove value if control is hidden
								if(this.hidden) return "";
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