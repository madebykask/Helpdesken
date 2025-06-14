
BEGIN TRAN

DECLARE  @customerID INT = 4,
	@now DATETIME = GETDATE(),
	@createdBy NVARCHAR(3) = 'CHS',
	@vendorCategoryGuid UNIQUEIDENTIFIER = 'AD565179-E89D-41C0-8176-2A5B23FFD402'


INSERT INTO tblCaseSolution(Customer_Id, CaseSolutionName, CreatedDate, ChangedDate, [Status], ShowOnCaseOverview, ShowInsideCase, OverWritePopUp, CaseSolutionGUID, CaseSolutionDescription) 
SELECT @customerID, 'Vendor case', @now, @now, 1, 1, 1, 1, '2E6C8CEE-DE05-4418-B2D9-34D9F3A94C71', 'Case solution for vendor'

DECLARE @caseSolutionID INT = SCOPE_IDENTITY()

INSERT ExtendedCaseForms([Description], CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, [Status], [Name], MetaData)
SELECT 'Vendor case - Initiator', @now,  @createdBy, @now, @createdBy, 1, 'Vendor case - Initiator',
'{
	id: 999,
	name: "Vendor case",
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
						return window.parent.document.getElementById(''case__ReportedBy'').value;
					}
				}, {
					id: "Address",
					type: "label",
					label: "Address",
					hiddenBinding: function(m, log) {
						return false;
					},
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDAddress.length === 0)
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
				column: 1,
				controls: [{
						id: "Email",
						type: "dropdown",
						label: "Select contact email",
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
								if (this.value == '''') {
									if (m.dataSources.getCBDEmails.length == 0)
										return "-";
									return m.dataSources.getCBDEmails[0].ADDRESS;
								} else {
									return this.value;
								}
							}
							/*},
							{
							id: "EmailLabel",
							type: "label",
							label: "",
							//noDigest: true
							valueBinding: function(m, log) {
								return m.tabs.Contact.sections.Contact.controls.Email.value;
							}
							}]*/
					},
					/*	{
					id: "Phone",
					name: "Phone",
					column: 1,
					controls: [*/
					{
						id: "PhoneEmail",
						type: "label",
						label: "Contact",
						hiddenBinding: function(m, log) {
							return false;
						},
						//noDigest: true
						valueBinding: function(m, log) {
							/**if (m.dataSources.getCBDPhone.length == 0)
								return "-";*/
							var label = ''Phone: '' + (m.dataSources.getCBDPhone.length == 0 ? ''-'' : m.dataSources.getCBDPhone[0].ADDRESS) + '', Email: '' + m.tabs.Contact.sections.Contact.controls.Email.value;
							return label;
						}
					}
				]
			}]
		}, {
			id: "TaxReg",
			name: "Tax registration",
			sections: [{
				id: "TaxReg",
				name: "Tax registration",
				controls: [{
					id: "TaxRegNo",
					type: "dropdown",
					label: "Select tax registration number",
					hiddenBinding: function(m, log) {
						return false;
					},
					//noDigest: true
					dataSource: {
						type: "custom",
						id: "getCBDTaxReg",
						valueField: "TAX_REG_NO",
						textField: "TAX_REG_NO",
						parameters: [{
							name: "cluCode",
							field: "tabs.Address.sections.Address.controls.BusinessUnit"
						}]
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (this.value == '''') {
							if (m.dataSources.getCBDTaxReg.length == 0)
								return "-";
							return m.dataSources.getCBDTaxReg[0].TAX_REG_NO;
						} else {
							return this.value;
						}
					}
				}, {
					id: "TaxRegNoLabel",
					type: "label",
					label: "Tax registration number",
					hiddenBinding: function(m, log) {
						return false;
					},

					valueBinding: function(m, log) {
						return m.tabs.TaxReg.sections.TaxReg.controls.TaxRegNo.value;
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
						var template = ''{bank-name} (IBAN: {iban}, SWIFT: {swift})'';
						var str = template.replace(''{bank-name}'', bank.BANK_NAME == '''' || bank.BANK_NAME == null ? ''-'' : bank.BANK_NAME)
							.replace(''{iban}'', bank.IBAN == '''' || bank.IBAN == null ? ''-'' : bank.IBAN)
							.replace(''{swift}'', bank.SWIFT == '''' || bank.SWIFT == null ? ''-'' : bank.SWIFT);
						return str;
					}
				}]
			}]
		}
	]
}'

DECLARE @initiatorFormID INT = SCOPE_IDENTITY()


INSERT ExtendedCaseForms([Description], CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, [Status], [Name], MetaData)
SELECT 'Vendor case - Regarding', @now,  @createdBy, @now, @createdBy, 1, 'Vendor case - Regarding',
'{
	id: 998,
	name: "Vendor case",
	localization: {
		dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
		decimalSeparator: ","
	},
	styles: ".sectionHeader { display: none; margin-top: 15px; } .tab-content { margin-top: 20px; }",
	dataSources:
	[
		{
			type: "query",			
			id: "getCBDAddress",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDEmails",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDPhone",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDTaxReg",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDBank",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		}
	],
	validatorsMessages: {
		required: "",
		alternativeRequired: "",
		dateFormat: "Correct date format (dd.mm.yyyy)",
		numeric: "Only numeric values allowed"
	},
	tabs: [
		{
			id: "Vendor",
			name: "Vendor",
			sections: [{
					id: "VendorInformation",
					name: "Vendor information",

					controls: [
						{
							id: "BusinessKey",
							type: "label",
							label: "Business key",
							hiddenBinding: function(m, log){return false;},
							valueBinding: function(m, log){
								return window.parent.document.getElementById(''case__IsAbout_ReportedBy'').value;
							}
						 },
						{
							id: "Address",
							type: "label",
							label: "Address",
							hiddenBinding: function(m, log){return false;},
							valueBinding: function(m, log){
								if (m.dataSources.getCBDAddress.length == 0)
									return "-";
								
								var template = ''{0}, {1}, {2}, {3}''

								var address = m.dataSources.getCBDAddress[0];

								var addressStr = template.replace(''{0}'', address.STREET)
													.replace(''{1}'', address.ZIP_CODE)
													.replace(''{2}'', address.CITY)
													.replace(''{3}'', address.GA_CODE_CTY);
								
								
								return addressStr;
							}
						 }
					]
				}
			]

		},
		
		{
			id: "Contact",
			name: "Contact",
			sections: [{
				id: "Contact",
				name: "Contact",
				controls: [{
					id: "Email",
					type: "dropdown",
					label: "Email",
					dataSource: {
						type: "custom",
						id: "getCBDEmails",
						hiddenBinding: function(m, log){return false;},
						parameters: [{ name: "cluCode", field: "tabs.Address.sections.Address.controls.BusinessUnit"}],
						valueField: "ADDRESS", 
						textField: "ADDRESS"
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (this.value == ''''){
							if (m.dataSources.getCBDEmails.length == 0)
								return "-";
							return m.dataSources.getCBDEmails[0].ADDRESS;
						}
						else
						{
							return this.value;
						}
					}
					},
					{
					id: "Phone",
					type: "label",
					label: "Phone",
					hiddenBinding: function(m, log){return false;},
					//noDigest: true
					valueBinding: function(m, log) {
									if (m.dataSources.getCBDPhone.length == 0)
										return "-";
									return m.dataSources.getCBDPhone[0].ADDRESS;
					}
					}]
			}]
		},
		{
			id: "TaxReg",
			name: "Tax registration",
			sections: [{
				id: "TaxReg",
				name: "Tax registration",
				controls: [{
						id: "TaxRegNo",
						type: "dropdown",
						label: "Tax registration number",
						hiddenBinding: function(m, log){return false;},
						//noDigest: true
						dataSource: {
							type: "custom",
							id: "getCBDTaxReg",
							valueField: "TAX_REG_NO",
							textField: "TAX_REG_NO",
							parameters: [{ name: "cluCode", field: "tabs.Address.sections.Address.controls.BusinessUnit"}]
						},
						//noDigest: true
						valueBinding: function(m, log) {
							if (this.value == ''''){
								if (m.dataSources.getCBDTaxReg.length == 0)
									return "-";
								return m.dataSources.getCBDTaxReg[0].TAX_REG_NO;
							}
							else {
								return this.value;
							}
						}
					}]
			}]
		},
		{
			id: "Bank",
			name: "Bank details",
			sections: [{
				id: "Bank",
				name: "Bank details",
				controls: [{
						id: "BankAccount",
						type: "label",
						label: "Bank account",
						hiddenBinding: function(m, log){return false;},
						//noDigest: true
						valueBinding: function(m, log) {
										console.log(m);
										if (m.dataSources.getCBDBank.length == 0)
											return "-";
										return m.dataSources.getCBDBank[0].ACCOUNT_NO;
						}
					},
					{
						id: "BankName",
						type: "label",
						label: "Bank",
						hiddenBinding: function(m, log){return false;},
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
					}
				]
			}]
		}
	]
}'

DECLARE @regardingFormID INT = SCOPE_IDENTITY()

INSERT ExtendedCaseForms([Description], CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, [Status], [Name], MetaData)
SELECT 'ComputerUser - Vendor', @now,  @createdBy, @now, @createdBy, 1, 'ComputerUser - Vendor',
'{
	id: 997,
	name: "Vendor Computeruser",
	localization: {
		dateFormat: "DD.MM.YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
		decimalSeparator: ","
	},
	styles: ".sectionHeader { display: none; margin-top: 15px; } .tab-content { margin-top: 20px; }",
	dataSources:
	[
		{
			type: "query",			
			id: "getCBDAddress",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDEmails",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDPhone",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDTaxReg",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		},
		{
			type: "query",			
			id: "getCBDBank",
			parameters: [{ name: "cluCode", field: "tabs.Vendor.sections.VendorInformation.controls.BusinessKey"}]
		}
	],
	validatorsMessages: {
		required: "",
		alternativeRequired: "",
		dateFormat: "Correct date format (dd.mm.yyyy)",
		numeric: "Only numeric values allowed"
	},
	tabs: [
		{
			id: "Vendor",
			name: "Vendor",
			sections: [{
					id: "VendorInformation",
					name: "Vendor information",

					controls: [
						{
							id: "BusinessKey",
							type: "label",
							label: "Business key",
							hiddenBinding: function(m, log){return false;},
							valueBinding: function(m, log){
								return window.parent.document.getElementById(''UserId_Value'').value;
							}
						 },
						{
							id: "Address",
							type: "label",
							label: "Address",
							hiddenBinding: function(m, log){return false;},
							valueBinding: function(m, log){
								if (m.dataSources.getCBDAddress.length == 0)
									return "-";
								
								var template = ''{0}, {1}, {2}, {3}''

								var address = m.dataSources.getCBDAddress[0];

								var addressStr = template.replace(''{0}'', address.STREET)
													.replace(''{1}'', address.ZIP_CODE)
													.replace(''{2}'', address.CITY)
													.replace(''{3}'', address.GA_CODE_CTY);
								
								
								return addressStr;
							}
						 }
					]
				}
			]

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
						hiddenBinding: function(m, log){return false;},
						parameters: [{ name: "cluCode", field: "tabs.Address.sections.Address.controls.BusinessUnit"}],
						valueField: "ADDRESS", 
						textField: "ADDRESS"
					},
					//noDigest: true
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDEmails.length == 0)
							return "-";

						var emails = [];

						for(var i = 0; i < m.dataSources.getCBDEmails.length; i++)
						{
							emails.push(m.dataSources.getCBDEmails[i].ADDRESS);
						}
						var result =  emails.join('', '');

						return result;
					}
					},
					{
					id: "Phone",
					type: "label",
					label: "Phone",
					hiddenBinding: function(m, log){return false;},
					//noDigest: true
					valueBinding: function(m, log) {
						if (m.dataSources.getCBDPhone.length == 0)
							return "-";
						return m.dataSources.getCBDPhone[0].ADDRESS;
					}
					}]
			}]
		},
		{
			id: "TaxReg",
			name: "Tax registration",
			sections: [{
				id: "TaxReg",
				name: "Tax registration",
				controls: [{
						id: "TaxRegNo",
						type: "label",
						label: "Tax registration number",
						hiddenBinding: function(m, log){return false;},
						//noDigest: true
						dataSource: {
							type: "custom",
							id: "getCBDTaxReg",
							valueField: "TAX_REG_NO",
							textField: "TAX_REG_NO",
							parameters: [{ name: "cluCode", field: "tabs.Address.sections.Address.controls.BusinessUnit"}]
						},
						//noDigest: true
						valueBinding: function(m, log) {
							if (m.dataSources.getCBDTaxReg.length == 0)
								return "-";
	
							var taxReg = [];
	
							for(var i = 0; i < m.dataSources.getCBDTaxReg.length; i++)
							{
								taxReg.push(m.dataSources.getCBDTaxReg[i].TAX_REG_NO);
							}
							var result =  taxReg.join('', '');
	
							return result;
						}
					}]
			}]
		},
		{
			id: "Bank",
			name: "Bank details",
			sections: [{
				id: "Bank",
				name: "Bank details",
				controls: [{
						id: "BankAccount",
						type: "label",
						label: "Bank account",
						hiddenBinding: function(m, log){return false;},
						//noDigest: true
						valueBinding: function(m, log) {
										console.log(m);
										if (m.dataSources.getCBDBank.length == 0)
											return "-";
										return m.dataSources.getCBDBank[0].ACCOUNT_NO;
						}
					},
					{
						id: "BankName",
						type: "label",
						label: "Bank",
						hiddenBinding: function(m, log){return false;},
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
					}
				]
			}]
		}
	]
}'

DECLARE @computerUserCategoryFormID INT = SCOPE_IDENTITY()


-- Connect case solution to section and extended case form for initiator
INSERT INTO tblCaseSolution_tblCaseSection_ExtendedCaseForm(tblCaseSolutionID, tblCaseSectionID, ExtendedCaseFormID)
SELECT @caseSolutionID, CS.Id, @initiatorFormID FROM tblCaseSections CS
WHERE CS.Customer_Id = @customerID AND CS.SectionType = 0 /* Initiator */

-- Connect case solution to section and extended case form for regarding
INSERT INTO tblCaseSolution_tblCaseSection_ExtendedCaseForm(tblCaseSolutionID, tblCaseSectionID, ExtendedCaseFormID)
SELECT @caseSolutionID, CS.Id, @regardingFormID FROM tblCaseSections CS
WHERE CS.Customer_Id = @customerID AND CS.SectionType = 1 /* Regarding */

-- Insert computer user category
INSERT INTO tblComputerUsersCategory([Name], CaseSolutionID, ComputerUsersCategoryGuid, IsReadOnly, CustomerID, ExtendedCaseFormID)
SELECT 'Vendor', @caseSolutionID, @vendorCategoryGuid, 1, @customerID, @computerUserCategoryFormID

DECLARE @vendorCategory INT = SCOPE_IDENTITY()

INSERT INTO ExtendedCaseCustomDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDPhone', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_Phone" }', @now, @createdBy, @now, @createdBy

INSERT INTO ExtendedCaseCustomDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDEmails', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_Emails" }', @now, @createdBy, @now, @createdBy

INSERT INTO ExtendedCaseCustomDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDAddress', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_Address" }', @now, @createdBy, @now, @createdBy

INSERT INTO ExtendedCaseCustomDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDTaxReg', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_TaxReg" }', @now, @createdBy, @now, @createdBy

INSERT INTO ExtendedCaseCustomDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDBank', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_Bank" }', @now, @createdBy, @now, @createdBy

INSERT INTO ExtendedCaseCustomDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDTaxRegType', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_TaxRegType" }', @now, @createdBy, @now, @createdBy

-- Optional datasources
INSERT INTO ExtendedCaseOptionDataSources(DataSourceId, [Description], MetaData, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
SELECT 'getCBDEmails', 'db-sp', '{ Type: "db-sp", ProcedureName: "CBD_Get_Emails" }', @now, @createdBy, @now, @createdBy

SELECT * FROM ExtendedCaseOptionDataSources

COMMIT

--INSERT INTO tblCaseSections(Customer_Id, SectionType, IsNewCollapsed, IsEditCollapsed, CreatedDate)
--SELECT 39, SectionType, IsNewCollapsed, IsEditCollapsed, GETDATE() FROM tblCaseSections CS
--WHERE CS.Customer_Id = 43




