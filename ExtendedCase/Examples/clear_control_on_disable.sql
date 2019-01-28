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
                            valueBinding: function(m, log){								
                                var val = this.parent.controls.DeductionsAmountUnits.value;
                                return val === undefined || val == "" ? "Amount" : undefined; 
                            },
                            dataSource: [{value: "Amount", text: "Amount"}, {value: "Unit", text: "Unit"}]
                        },
                        {
					   id: "DeductionsAmount_Amount",
					   type: "amount",
					   label: "",
                            addonText: "ˆ",
                            disabledBinding: function(m, log){								
                                return this.parent.controls.DeductionsAmountUnits.value !== "Amount";
                            },
					   valueBinding: function(m) {
						  // remove value if control if hidden
						  if(this.disabled) return "";
					   }
                        },
                        {
					   id: "DeductionsAmount_Unit",
					   type: "unit",
					   label: "",
                            disabledBinding: function(m, log){
                                return this.parent.controls.DeductionsAmountUnits.value !== "Unit";
                            },
					   valueBinding: function(m) {
						  // remove value if control is hidden
						  if(this.disabled) return "";
					   }
                        }
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
                    name: "Review",                                
                    reviewSectionId: "tabs.PaymentInformation.sections.Deductions",                                                                
                    controls: [                        
                        {
                            reviewControlId: "DeductionsAmount_Amount",
							label: "Amount",
                            valueBinding: function(m, log) {
                                return ["", this.value];
                            }
                        },
                        {
                            reviewControlId: "DeductionsAmount_Unit",                                        
							label: "Unit",
							valueBinding: function(m, log) {
                                return ["", this.value];
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