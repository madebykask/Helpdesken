DECLARE @metaData nvarchar(max)

--INSERT ExtendedCaseForms([Description], CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, [Status], [Name], MetaData)
--SELECT 'Vendor case - Initiator', @now,  @createdBy, @now, @createdBy, 1, 'Vendor case - Initiator',

SET @metaData = '{
	id: 997,
	name: "Vendor Computeruser",
	localization: {
		dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
		decimalSeparator: ","
	},
	styles: ".sectionHeader { display: none; margin-top: 15px; } .tab-content { margin-top: 20px; }",
	dataSources: [{
		type: "query",
		id: "getCBDAddress",
		parameters: [{
			name: "cluCode",
			field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"
		}]
	}, {
		type: "query",
		id: "getCBDEmails",
		parameters: [{
			name: "cluCode",
			field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"
		}]
	}, {
		type: "query",
		id: "getCBDPhone",
		parameters: [{
			name: "cluCode",
			field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"
		}]
	}, {
		type: "query",
		id: "getCBDTaxReg",
		parameters: [{
			name: "cluCode",
			field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"
		}]
	}, {
		type: "query",
		id: "getCBDBank",
		parameters: [{
			name: "cluCode",
			field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"
		}]
	}],
	validatorsMessages: {
		required: "",
		alternativeRequired: "",
		dateFormat: "Correct date format (dd.mm.yyyy)",
		numeric: "Only numeric values allowed"
	},
	tabs: [{
			id: "Vendor",
			name: "Vendor",
			sections: [{
				id: "VendorInformation",
				name: "Vendor information",

				controls: [{
					id: "BusinessKey",
					type: "label",
					label: "Business key",
					hiddenBinding: function(m, log) {
						return false;
					},
					valueBinding: function(m, log) {
						return window.parent.document.getElementById(''UserId_Value'').value;
					}
				}, {
					id: "Address",
					type: "label",
					label: "Address",
					hiddenBinding: function(m, log) {
						return false;
					},
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDAddress.length == 0)
							return "-";

						var template = ''{0}, {1}, {2}, {3}'';

						var address = m.dataSources.getCBDAddress[0];

						var addressStr = template.replace(''{0}'', address.STREET)
							.replace(''{1}'', address.ZIP_CODE)
							.replace(''{2}'', address.CITY)
							.replace(''{3}'', address.GA_CODE_CTY);


						return addressStr;
					}
				}]
			}]

		},

		{
			id: "Contact",
			name: "Contact",
			sections: [{
				id: "Contact",
				name: "Contact",
				controls: [{
					id: "Email",
					type: "label",
					label: "Email",
					dataSource: {
						type: "custom",
						id: "getCBDEmails",
						hiddenBinding: function(m, log) {
							return false;
						},
						parameters: [{
							name: "cluCode",
							field: "tabs.Address.sections.Address.controls.BusinessUnit"
						}],
						valueField: "ADDRESS",
						textField: "ADDRESS"
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDEmails.length == 0)
							return "-";

						var emails = [];

						for (var i = 0; i < m.dataSources.getCBDEmails.length; i++) {
							emails.push(m.dataSources.getCBDEmails[i].ADDRESS);
						}

						var result = emails.join('', '');
						return result;
					}
				}, {
					id: "Phone",
					type: "label",
					label: "Phone",
					hiddenBinding: function(m, log) {
						return false;
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDPhone.length == 0)
							return "-";
						return m.dataSources.getCBDPhone[0].ADDRESS;
					}
				}]
			}]
		}, {
			id: "TaxReg",
			name: "Tax registration",
			sections: [{
				id: "TaxReg",
				name: "Tax registration",
				controls: [{
					id: "TaxRegNo",
					type: "label",
					label: "Tax registration number",
					hiddenBinding: function(m, log) {
						return false;
					},
					//noDigest: true
					dataSource: {
						type: "custom",
						id: "getCBDTaxReg",
						valueField: "TAX_REG_NO",
						textField: "FormattedTax",
						parameters: [{
							name: "cluCode",
							field: "tabs.Address.sections.Address.controls.BusinessUnit"
						}]
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDTaxReg.length == 0)
							return "-";

						var taxReg = [];

						for (var i = 0; i < m.dataSources.getCBDTaxReg.length; i++) {
							taxReg.push(m.dataSources.getCBDTaxReg[i].FormattedTax);
						}

						var result = taxReg.join('', '');
						return result;
					}
				}]
			}]
		}, {
			id: "Bank",
			name: "Bank details",
			sections: [{
				id: "Bank",
				name: "Bank details",
				controls: [{
					id: "BankAccount",
					type: "label",
					label: "Bank account",
					hiddenBinding: function(m, log) {
						return false;
					},
					//noDigest: true
					valueBinding: function(m, log) {
						console.log(m);
						if (m.dataSources.getCBDBank.length == 0)
							return "-";
						return m.dataSources.getCBDBank[0].ACCOUNT_NO;
					}
				}, {
					id: "BankName",
					type: "label",
					label: "Bank",
					hiddenBinding: function(m, log) {
						return false;
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDBank.length == 0)
							return "-";
						var bank = m.dataSources.getCBDBank[0];
						var template = ''{bank-name} (IBAN: {iban}, SWIFT: {swift})''
						var str = template.replace(''{bank-name}'', bank.BANK_NAME == '''' || bank.BANK_NAME == null ? ''-'' : bank.BANK_NAME)
							.replace(''{iban}'', bank.IBAN == '''' || bank.IBAN == null ? ''-'' : bank.IBAN)
							.replace(''{swift}'', bank.SWIFT == '''' || bank.SWIFT == null ? ''-'' : bank.SWIFT)
						return str;
					}
				}]
			}]
		}
	]
}'


--select * from [ExtendedCaseForms]

UPDATE [dbo].[ExtendedCaseForms]
SET MetaData = @metaData
WHERE Id = <form_Id>