EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO
DELETE FROM [dbo].[ExtendedCaseForms] WHERE id=1

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
           (1, '{
	id: 1,
	name: "@Translation.Name",
	localization: {
		dateFormat: "DD-MM-YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
		decimalSeparator: ","
	},
	globalFunctions: {
		test1: function(m, log) { return m.tabs.tab1.sections.sec1.controls.id01.value; },
		test2: function(m, log) { return "Warning! Test"; },
		test3: function(m) { return m.tabs.tab1.sections.sec1.controls.id1.value },
		populateBindingTest: function(m) {
									return {
										"sAddressLine1": m.tabs.tab4.sections.permAddressSec.controls.pAddressLine1.value,
										"sAddressLine2": m.tabs.tab4.sections.permAddressSec.controls.pAddressLine2.value,                        
										"sPostCode"    : m.tabs.tab4.sections.permAddressSec.controls.pPostCode.value,
										"sCity"        : m.tabs.tab4.sections.permAddressSec.controls.pCity.value,
										"multiselect3" : m.tabs.tab4.sections.permAddressSec.controls.multiselect2.value
									}
								},
		isRequiredEnabled: function(m) {
			return m.nextStep !== 5; 
		 }	
	},
    dataSources:	
	[
		//static data
		{
			type: "static",
			id : "SomeDataSource1",            
			data: [{ prop1: "value1" }, { prop2: "value2"}]
		},       
		
		//avaialble users
		{
			type: "query",			
            id: "getAvaialableUsers"			
        },

		//customers for selected user
		{
			type: "query",			
            id: "customersByUserQuery",
			parameters: [{ name: "userId", field: "tabs.tab1.sections.sec1.controls.usersDropDown1"}]            
        },

		// departments for selected customer
		{
			type: "query",			
			id: "customerDepartmentsQueryDs",
			parameters: [{ name: "customerId", field: "tabs.tab1.sections.sec1.controls.customerDropDown1"}]
		},

		//departments
		{
			type: "query",			
            id: "departmentsByUserQuery",
			parameters: [{ name: "value", field: "tabs.tab1.sections.sec2.controls.id41"}]
        },
    ],
	validatorsMessages: {
		required: "Field is required.",
		alternativeRequired: "Field is required(alternative).",
		dateFormat: "Incorrect format.Expect DD-MM-YYYY"
	},
	tabs: [{
		id: "tab1",
		name: "Tab 1",
		sections: [{
			id: "sec1",
			name: "Section 1",
            column: 0,
			controls: [
            //case field
            {
				id: "id01",
				type: "textbox",
				label: "TextBox Case 1",
                shouldNotSave: true,
                caseBinding: "caseField1"
			},

            //next step
            {
				id: "id02",
				type: "textbox",
				label: "TextBox Next Step",
                valueBinding: function(m, log) {
                    if (!m.nextStep)
                        return "no next step";
                    return m.nextStep;
                }
			},

            //user Guid
            {
				id: "id03",
				type: "textbox",
				label: "TextBox User Guid",
                valueBinding: function(m, log) {
                    if (!m.formInfo.userGuid)
                        return "no userGuid";
                    return m.formInfo.userGuid;
                },
				warningBinding: "test2"
			},

			//global function call
            {
				id: "id04",
				type: "textbox",
				label: "TextBox Global Func 1",
                valueBinding: function(m, log) {
                    return m.globalFunctions.test3(m);
                }
			},
			
			//users
			{
				id: "usersDropDown1",
				type: "dropdown",
				label: "Select User",
				dataSource: {
                    type: "custom",
                    id: "getAvaialableUsers",
					valueField: "Id",
					textField: "FirstName"
                },
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
			
			//customers for selected user
			{
				id: "customerDropDown1",
				type: "dropdown",
				label: "Select Customer",
				dataSource: {
                    type: "custom",
                    id: "customersByUserQuery",
					valueField: "Id",
					textField: "Name"
                },
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},

			//select email of the selected customer from the loaded datasource
			{
				id: "txtSelectedCustomerEmail",
				type: "textbox",
				label: "Customer Email:",
				valueBinding: function(m, log){					
					var ds = m.dataSources.customersByUserQuery;
					var customersDropDown = m.tabs.tab1.sections.sec1.controls.customerDropDown1;
					if (ds && customersDropDown && customersDropDown.value !== undefined){										
						var selectedCustomer = ds.find(function(el){
							if (el.Id === customersDropDown.value)
								return el;
						});	
													
						if (selectedCustomer && selectedCustomer.HelpdeskEmail !== undefined){
							return selectedCustomer.HelpdeskEmail;
						}	
					}
					else
						return "";										
				},				
			    hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id1.value == "555";
				},
				validators: {
					onNext: [
						{
							type: "required"							
						}										
					]
				}
			},
			
			{
				id: "id1",
				type: "textbox",
				label: "TextBox 1",
				valueBinding: "test1",
                /*validators: {
					onSave: [
						{
							type: "required", // predefined types (required, min, max &etc.) or custom
							enabled: function(m) { return m.tabs.tab1.sections.sec1.controls.usersDropDown1.value !== "2" }, // exists - use, not - default true
							//message: "", // if exist - use it, not exist - check messageName,
							messageName: "alternativeRequired" // if exist - get from global validatorsMessages by messageName value, not exist - get from global validatorsMessages by type name,						
						},
						{
							type: "custom",
							valid: function(m) {
								return m.tabs.tab1.sections.sec1.controls.usersDropDown1.value === "2";
							},
							message: "Custom onSave 1 Invalid."
						},									
					],
					onNext: [ // onNext validation always runs with onSave validation. onNext only adds validators to onSave
						{
							type: "maxlength",
							value: "24"
						},
						{
							type: "custom",
							valid: function(m) {
								return m.tabs.tab1.sections.sec1.controls.usersDropDown1.value === "2";
							},
							message: "Custom onNext Invalid."
						}
					]
				}*/
			},
			
			{
				id: "id11",
				type: "textbox",
				label: "TextBox 11",
				valueBinding: function(m, log){
					var ds = m.dataSources.customerDepartmentsQueryDs;
                    if (ds && ds.length > 0)
						return ds[0].Department;
					else 
						return "";
				},				
			    hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id1.value == "555";
				},
				/*validators:{
				onSave: [{
							type: "required"
						},
						{
							type: "custom",
							enabled: function(m) { return true; },
							valid: function(m) { 
								return m.tabs.tab1.sections.sec1.controls.usersDropDown1.value === "2" &&
									m.tabs.tab1.sections.sec1.controls.id11.value === "1"
							},
							message: "Custom message"
						}	
					]
				}*/
			},
			{
				id: "id2",
				type: "textarea",
				label: "Textarea",
                valueBinding: function(m, log){
                    if (!log.contains("id1") && !log.containsPath("tabs.tab1.sections.sec1.controls.id3"))
                        return this.value;
					return m.tabs.tab1.sections.sec1.controls.id1.value + " " +m.tabs.tab1.sections.sec1.controls.id3.value;
				},
                hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id1.value == "667";
				},
                disabledBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id1.value == "668";
				},
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
			{
				id: "id21",
				type: "textbox",
				label: "TextBox(iban) 21",
                /*validators: {
					onSave: [
						{
							type: "pattern", // predefined types (required, min, max &etc.) or custom
							value: "NL\d{2}[ ]\d{4}[ ]\d{4}[ ]\d{4}[ ]\d{2}|NL\d{18}|^NL\d{2}[A-Z]{4}\d{10}$|^NL\d{2}[ ][A-Z]{4}[ ]\d{4}[ ]\d{4}[ ]\d{2}$|^[A-Z]{2}\d{2}[ ][A-Z]{4}[ ]\d{4}[ ]\d{4}[ ]\d{2}", // dont use ^ at begining and $ at end
							//enabled: function(m) { return m.tabs.tab1.sections.sec1.controls.usersDropDown1.value !== "2" }, // exists - use, not - default true
							message: "Wrong format (NLNN AAAA NNNN NNNN NN). Example: NL12 ABCD 1234 1234 12", // if exist - use it, not exist - check messageName,							
						}										
					]
				}*/
			},
			{
				id: "id3",
				type: "amount",
				label: "Amount 3",
				addonText: "$",
				validators: {
					onSave: [
						{
							type: "min",
							value: "10",
							message: "min 10"
						}										
					]
				}
			},
			{
				id: "id301",
				type: "textbox",
				label: "Amount textbox",
				addonText: "$",
				validators: {
					onSave: [
						{
							type: "pattern",
							value: "[\d]+",
							message: "pattern"
						}										
					]
				}
			},
			{
				id: "id13",
				type: "unknown",
				label: "Unknown test"
			},
			{
				id: "id131",
				type: "date",
				label: "Date(range) 131",
				warningBinding: function(m, log) {
					var today = moment().startOf("day");
					var date = moment(m.tabs.tab1.sections.sec1.controls.id131.value, m.localization.dateFormat)
					if(today.isAfter(date)) {
						return "Date is in the past!";
					}
				},
				validators: {
					onSave: [
						{
							type: "required"
						}
						/*{
							type: "range",
							value: ["12/06/2017", "12/07/2017"],
							message: "range date 12/06/2017-12/07/2017"
						}*/										
					]
				}
			},
			{
				id: "id132",
				type: "date",
				label: "Date(min,max) 132",
				validators: {
					onSave: [
						{
							type: "min",
							value: "12/06/2017",
							message: "min 12/06/2017"
						},
						{
							type: "max",
							value: "12/07/2017",
							message: "max 12/07/2017"
						}										
					]
				}
			},
			{
				id: "id14",
				type: "label",
				valueBinding: function(m, log) {
					return "Label(14): Some text here Some text  ";
				}
			},
			{
				id: "id15",
				type: "percentage",
				label: "Percentage",
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
			{
				id: "id16",
				type: "unit",
				label: "Unit",
				addonText: "Unit1",
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},			
			{
				id: "id17",
				type: "checkbox-list",
				label: "checkboxes",
                dataSource: [{value: "1", text: "11"}, {value: "2", text: "22"}, {value: "3", text: "33"}],
                dataSourceFilterBinding: function(m, log, ds){
					if (m.tabs.tab1.sections.sec1.controls.id1.value == "11") {
                       ds.pop();
                       return ds;
                    }
                    if (m.tabs.tab1.sections.sec1.controls.id1.value == "112") {
                       ds.pop();
                       ds.push({value: "4", text: "44"});
                       return ds;
                    } 
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
				validators: {
					onSave: [
						{
							type: "required"
						},
						{
							type: "custom",
							valid: function(m) { 								
								return m.tabs.tab1.sections.sec1.controls.id17.value[1] && m.tabs.tab1.sections.sec1.controls.id17.value[3];
							},
							message: "1 and 3 should be selected."
						}										
					]
				}
			},			
			{
				id: "id18",
				type: "radio",
				label: "Radio",
				dataSource: [{value: "1", text: "11"}, {value: "2", text: "22"}, {value: "3", text: "33"}],
				validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}
			},			
			{
				id: "id19",
				type: "checkbox",
				label: "single checkbox",
				validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}
			}]
		},
		{
			id: "sec2",
			name: "Section 2",
            column: 1,
            hiddenBinding: function(m, log){
				return m.tabs.tab1.sections.sec1.controls.id1.value == "667";
			},
            disabledBinding: function(m, log){
				return m.tabs.tab1.sections.sec1.controls.id1.value == "668";
			},
			controls: [{
				id: "id4",
				type: "search",
				label: "Search/option 4",
				dataSource: {
                    type: "option",
                    id: "Search-query",
                    parameters: [
                        { name: "value", field: "tabs.tab1.sections.sec2.controls.id4"}
                    ]
                },
                dataSourceFilterBinding: function(m, log, ds){
					return ds;
				},
                hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id3.value == "667";
				},
                disabledBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id3.value == "668";
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
			{
				id: "id41",
				type: "search",
				label: "Search/CustomDataSource 41",
				dataSource: {
                    type: "custom",
                    id: "departmentsByUserQuery",
					valueField: "Value",
					textField: "Text"
                },
                dataSourceFilterBinding: function(m, log, ds){
					return ds;
				},
                hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id3.value == "667";
				},
                disabledBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id3.value == "668";
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},			            
			{
				id: "id42",
				type: "search",
				label: "Search/StaticDataSource 42",
				dataSource: [{value: "1", text: "111"}, {value: "2", text: "122"}, {value: "3", text: "133"}],
                dataSourceFilterBinding: function(m, log, ds){
					if(m.tabs.tab1.sections.sec2.controls.id42.value.length === 0) return ds;					
				    let query = new RegExp("^" +m.tabs.tab1.sections.sec2.controls.id42.value +".*$", "i");
					return ds.filter(function(item){
						return query.test(item.text);
					});
				},
                hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id3.value == "667";
				},
                disabledBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id3.value == "668";
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
			{
				id: "id51",
				type: "dropdown",
				label: "DropDown/StaticDataSource 51",
							dataSource: [],
							/*dataSourceFilterBinding: function(m, log, ds){
								if (m.tabs.tab1.sections.sec1.controls.id1.value == "117") {
					 return [{value: "2", text: "22"}, {value: "3", text: "33"}];
								} else {
									return ds;
								}

				},
							resetValueOnItemsUpdate: true,
				warningBinding: function(m, log) {
					
				 return "Warning! Test";
				},*/
				validators: {
				 onSave: [
				  {
				   type: "required"
				  }          
				 ]
				}
			},
            {
				id: "id52",
				type: "dropdown",
				label: "DropDown/option 52",
                dataSource: {
                    type: "option",
                    id: "Regions-get-query"
                },
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
            {
				id: "id53",
				type: "dropdown",
				label: "DropDown/option 53",
                dataSource: {
                    type: "option",
                    id: "Regions-query",
                    parameters: [
                        { name: "regionId", field: "tabs.tab1.sections.sec2.controls.id52"}
                    ]
                },
                dataSourceFilterBinding: function(m, log, ds){
					return ds;
				},
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},			
			{
				id: "id541",
				type: "multiselect",
				label: "multi select/static 541",
				dataSource: [{value: "1", text: "11"}, {value: "2", text: "22"}, {value: "3", text: "33"}],
				disabledBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id1.value == "5411";
				},
				hiddenBinding: function(m, log){
					return m.tabs.tab1.sections.sec1.controls.id1.value == "5421";
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
			},
			{
				id: "id542",
				type: "multiselect",
				label: "multi select/custom 542",
				dataSource: {
                    type: "custom",
                    id: "departmentsByUserQuery",
					valueField: "Value",
					textField: "Text"
                },
				dataSourceFilterBinding: function(m, log, ds){					
				    let query = new RegExp("^ikea.*$", "ig");
					return ds.filter(function(item){
						return query.test(item.text);
					});
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
			},
            {
				id: "id54",
				type: "dropdown",
				label: "DropDown/option 54",
                dataSource: {
                    type: "option",
                    id: "Regions-query",
                    parameters: [
                        { name: "regionId", field: "tabs.tab1.sections.sec2.controls.id52"}
                    ]
                },
                dataSourceFilterBinding: function(m, log, ds){
					return ds;
				},
				warningBinding: function(m, log) {
					return "Warning! Test";
				},
				/*validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			},
			{
				id: "id6",
				type: "date",
				label: "Date 6",
				disabledBinding: function(m) {
					return true;
				},
				/*validators: {
					onSave: [
						{ type: "required" }
					]
				},
				validators: {
					onSave: [
						{
							type: "required"
						}										
					]
				}*/
			}]
		}]
	},
	   {
		id: "tab2",
		name: "Tab 2",
        columnCount: 3,
        hiddenBinding: function(m, log){
			return m.tabs.tab1.sections.sec1.controls.id1.value == "667";
		},
        disabledBinding: function(m, log){
			return m.tabs.tab1.sections.sec1.controls.id1.value == "668";
		},
		sections: [{
			id: "sec21",
			name: "Section 21",
			controls: [{
				id: "id21",
				type: "textbox",
				label: "minLength 21",
				validators: {
					onSave: [
						{
							type: "minLength",
							value: "2",
							message:"minLength 2"
						}
					]
				}
			},
			{
				id: "id22",
				type: "textbox",
				label: "maxLength 22",
				validators: {
					onSave: [
						{
							type: "maxLength",
							value: "2",
							message:"maxLength 2"
						}										
					]
				}
			},
			{
				id: "id23",
				type: "textbox",
				label: "pattern 23",
				/*validators: {
					onSave: [
						{
							type: "required"
						},
						{
							type: "pattern", // predefined types (required, min, max &etc.) or custom
							value: "-?\d{0,4}(\,|.[0-9]{1,5})?", // dont use ^ at begining and $ at end
							//enabled: function(m) { return m.tabs.tab1.sections.sec1.controls.usersDropDown1.value !== "2" }, // exists - use, not - default true
							message: "Enter maximum four-digit value.", // if exist - use it, not exist - check messageName,							
						}										
					]
				}*/
			},
			{
				id: "id231",
				type: "textbox",
				label: "Textbox(min-max) 231",
				validators: {
					onSave: [
						{
							type: "min",
							value: "10",
							message:"min 10"
						},
						{
							type: "max",
							value: "20",
							message:"max 20"
						}
					]
				}
			},
			{
				id: "id232",
				type: "textbox",
				label: "Textbox(range) 232",
				validators: {
					onSave: [
						{
							type: "range",
							value: ["10", "20"],
							message:"range 10-20"
						}
					]
				}
			},
			{
				id: "id233",
				type: "textbox",
				label: "Textbox(warning) 233",
				warningBinding: function(m, log) {
					if(m.tabs.tab2.sections.sec21.controls.id233.value === "11") {
						return "Warning! Value is 11";
					}
				},
				validators: {
					onSave: [
						{
							type: "range",
							value: ["10", "20"],
							message:"range 10-20"
						}
					]
				}
			}]
		},
		{
			id: "sec22",
			name: "Section 22",
            column: 2,
			controls: [{
				id: "id24",
				type: "textbox",
				label: "TextBox 24"
			},
			{
				id: "id25",
				type: "textbox",
				label: "TextBox 25"
			},
			{
				id: "id26",
				type: "textbox",
				label: "TextBox 26"
			}]
		}]
	},
    {
		id: "tab3",
		name: "Tab 3",
        columnCount: 1,
		sections: [{
			id: "sec31",
			name: "Section 31",
			controls: [{
				id: "id311",
				type: "review",
				label: "TextBox 1",
                    shouldNotSave: false,
                    valueBinding: function(m, log) {
                       //return [m.tabs.tab1.sections.sec1.controls.usersDropDown1.value, m.tabs.tab1.sections.sec1.controls.usersDropDown1.secondaryValue];
				   return [m.tabs.tab1.sections.sec2.controls.id541.value, m.tabs.tab1.sections.sec2.controls.id541.secondaryValue];
				   
                    }
			}] 
        },
        {
			id: "sec32",
			name: "Section 32",
			controls: [
            {
				id: "id321",
				type: "review",
				label: "TextBox 1",
                valueBinding: function(n, log) {
                    return null;
                }
			}]
        },
        {
			id: "sec33",
			name: "Section 33",
			controls: [
            {
				id: "id331",
				type: "review",
				label: "TextBox 1",
                valueBinding: function(n, log) {
                    return "331";
                }
			}]
        },
        {
			id: "sec34",
			name: "Section 34",
			controls: [
            {
				id: "id341",
				type: "review",
				label: "TextBox 1",
                valueBinding: function(n, log) {
                    return "341";
                }
			}]
        }]
    },

     //tab4: begin
     {
          id: "tab4",
          name: "Tab4 (Populate Example)",
          columnCount: 3,
    
          sections: [{
               id: "permAddressSec",
               name: "Permanent Address",
                enableAction: {               
                    initialState: false,
                    label: "Toggle enable"
               },
               controls: [{
                    id: "pAddressLine1",
                    type: "textbox",
                    label: "Address Line1",                    
                  },
                  {
                    id: "pAddressLine2",
                    type: "textbox",
                    label: "Address Line2",                    
                  },                      
                                    
                  {
                     id: "pPostCode",
                     type: "textbox",
                     label: "Post Code"
                  },
                   
                   {
                        id: "pCity",
                        type: "dropdown",
                        label: "Select City",
                        dataSource: [{value: "1", text: "London"}, {value: "2", text: "Warsaw"}, {value: "3", text: "Paris"}]
                   },

                   {
                        id: "multiselect2",
                        type: "multiselect",
                        label: "multi select2",
                        dataSource: [{value: "1", text: "test1"}, {value: "2", text: "test2"}, {value: "3", text: "test3"}]
                   }]
            },
   
            {
               id: "shippingAddressSec",
               name: "Shipping Address",
               populateAction: { 
                actionName: "Populate",
                /*populateBinding: function(m){
                    return {
                        "sAddressLine1": m.tabs.tab4.sections.permAddressSec.controls.pAddressLine1.value,
                        "sAddressLine2": m.tabs.tab4.sections.permAddressSec.controls.pAddressLine2.value,                        
                        "sPostCode"    : m.tabs.tab4.sections.permAddressSec.controls.pPostCode.value,
                        "sCity"        : m.tabs.tab4.sections.permAddressSec.controls.pCity.value,
                        "multiselect3" : m.tabs.tab4.sections.permAddressSec.controls.multiselect2.value
                    };					
                }*/
				populateBinding: "populateBindingTest"
               },
               controls: [{
                    id: "sAddressLine1",
                    type: "textbox",
                    label: "Address Line1"                
                  },
                  
                  {
                    id: "sAddressLine2",
                    type: "textbox",
                    label: "Address Line2"                  
                  },
                  
                  {
                    id: "sPostCode",
                    type: "textbox",
                    label: "Post Code"                 
                  },
                  
                  {
                    id: "sCity",
                    type: "dropdown",
                    label: "Select City",
                    dataSource: [{value: "1", text: "London"}, {value: "2", text: "Warsaw"}, {value: "3", text: "Paris"}]             
                  },

                  {
                        id: "multiselect3",
                        type: "multiselect",
                        label: "multi select3",
                        dataSource: [{value: "1", text: "test1"}, {value: "2", text: "test2"}, {value: "3", text: "test3"}]
                  }
               ]
            }   
       ] //sections end   
    } //tab4: end   
    
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