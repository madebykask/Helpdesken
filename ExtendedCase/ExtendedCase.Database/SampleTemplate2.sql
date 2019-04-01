DECLARE @metaData nvarchar(MAX) = 
'{
     id: 2,
     name: "@Translation.Name",
     localization: {
		dateFormat: "DD-MM-YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
		decimalSeparator: ","
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
    ],
    validatorsMessages: {
		required: "Field is required.",
		alternativeRequired: "Field is required(alternative).",
		dateFormat: "Incorrect format.Expect DD-MM-YYYY"
	},
     tabs: [{
          id:   "tab1",
          name: "Tab 1",                 
          sections: [{

               id: "sec1",
               name: "Section 1",               
               enableAction: {               
                    initialState: false,
                    label: "Toggle enable"
               },      
               
               multiSectionAction: {
                    allowMultipleSections : true,
                    actionName: "add",
                    maxCount: 2
               },    
               
              /* hiddenBinding: function(m,log ){
                      if (this.first.controls.Search.secondaryValue != "") {
                            return true;
                        } else {
                            return false;
                        }
               },*/

               controls: [                
               //search
               {
                    id: "Search",
                    type: "search",
                    label: "Search",
                    //noDigest: true
                    dataSource: [{
                                    "value": "366397",
                                    "text": "00100015-ALVIN WATTERSON"
                                  },
                                  {
                                    "value": "366398",
                                    "text": "00105678-AARON COSTAIN"
                                  },
                                  {
                                    "value": "366399",
                                    "text": "00122245-CULVER CORKILL"
                                  },
                                  {
                                    "value": "366400",
                                    "text": "00122437-AARON COSTAIN"
                                  }]
                },

               //simple
               {
                    id: "id1",
                    type: "textbox",
                    label: "TextBox 1",
                    valueBinding: function(m, log) {
                                   if (m.tabs.tab1.sections.sec1.first.controls.Search.secondaryValue != "") {
                                        return m.tabs.tab1.sections.sec1.first.controls.Search.secondaryValue;
                                   } else {
                                        return 0;
                                   }
                      },

                      disabledBinding: function(m, log) {
                        if (log.logs.length > 0) {
						  var search = log.logs.find(function(d) {
							 return d.id === "tabs.tab1.sections.sec1.controls.Search";
						  });

                                if (search != null && search.newValue != search.oldValue) {
                                    return true;
                                }
                        }
                        return false;
                      }
               },
               /*
               {
                    id: "dateOfBirth",
                    label: "Date of birth",
                    type:"date",
                    validators: {
				    onSave: [
				        {
					       type: "required"
				        },
				        {
					       type: "custom",
					       valid: function(m) { 								
                                    //debugger;
                                    let date = this.value;
                                    //check age is over 18
                                    var momentDate = moment(date, m.localization.dateFormat);
                                    var allowedMomentDate = moment().subtract(18, "y");
                                    var res = momentDate.isBefore(allowedMomentDate);
                                    return res; 
					       },
					       message: "Your age should be over 18."
				        }										
				    ]
		          }//validators
               },*/
                /*
               {
                    id: "meetingDate",
                    label: "Meeting date",
                    type:"date",
                    validators: {
				    onSave: [
				        {
					       type: "required"
				        },
				        {
					       type: "custom",
					       valid: function(m) { 								
                                    //debugger;
                                    let date = this.value;
                                    
                                    var momentDate = moment(date, m.localization.dateFormat);
                                    var months = [3,6,9,12];

                                    //check date is 1 or 15 of every third month 
                                    if ((momentDate.date() == 1  || momentDate.date() == 15) &&                                               
                                         months.indexOf(momentDate.month() + 1) != -1)   {
                                         return true;
                                    }
                                    return false; 
					       },
					       message: "Meeting date should be 1 or 15 day of every 3rd month."
				        }										
				    ]
		          }//validators
               },
               */
               //check-box-list
               {
                    id: "chk1",
                    type: "checkbox-list",
                    label: "Select option",
                    dataSource: [{value: "val1", text: "test1"}, {value: "val2", text: "test2"}, {value: "val3", text: "test3"}]
               },

              //multi
               {
                    id: "multiselect1",
                    type: "multiselect",
                    label: "multi select",
                    dataSource: [{value: "1", text: "11"}, {value: "2", text: "22"}, {value: "3", text: "33"}]
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
                }
               },
               
               //customers for selected user
               {
                    id: "customerDropDown1",
                    type: "dropdown",
                    label: "Select Customer",
                      
                    // incorrect value test
                    /*valueBinding: function(m, log) {
                            
                            if (m.tabs.tab1.sections.sec1.first.controls.usersDropDown1.value !== "") {
                                return "9";
                            } else {
                                return "";
                            }
                    },*/

                    dataSource: {
                       type: "custom",
                       id: "customersByUserQuery",
                       valueField: "Id",
                       textField: "Name"
                    }
               },

               {			
                   id: "txtCaseNumber",
                   type: "textbox",
                   label: "Case Number",
                   caseBinding: "caseNumber",                   
                   validators:{
				onSave: [{ 
                        type: "required",
                        enabled: function() {
                            if (!this.parent.forceEnable) { return false; }
                            return true;
                        }
                     }]
				}
               },
               
               //select email of the selected customer from the loaded datasource
               {
                    id: "txtSelectedCustomerEmail",
                    type: "textbox",
                    label: "Customer Email:",

                    validators:{
				    onSave: [{ 
                            type: "required",
                            enabled: function() {
                                if (!this.parent.forceEnable) { return false; }
                                return true;
                            }  
                       }]
				},

                    disabledBinding: function(m, log){
                         return false;
                    },
                    valueBinding: function(m, ctx, log){					
                          var ds = m.dataSources.customersByUserQuery;
                         var customersDropDown = m.tabs.tab1.sections.sec1.first.controls.customerDropDown1;
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
                    }

               }] //controls
          }] //sections
     },
     /*
     //tab2: begin
     {
          id: "tab2",
          name: "Tab 2",
          columnCount: 3,
    
          sections: [{
               id: "permAddressSec",
               name: "Permanent Address",
               multiSectionAction: {
                    allowMultipleSections : true,
                    actionName: "add",
                    maxCount: 4
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
                populateBinding: function(m){
                    return {
                        "sAddressLine1": m.tabs.tab2.sections.permAddressSec.controls.pAddressLine1.value,
                        "sAddressLine2": m.tabs.tab2.sections.permAddressSec.controls.pAddressLine2.value,                        
                        "sPostCode"    : m.tabs.tab2.sections.permAddressSec.controls.pPostCode.value,
                        "sCity"        : m.tabs.tab2.sections.permAddressSec.controls.pCity.value,
                        "multiselect3" : m.tabs.tab2.sections.permAddressSec.controls.multiselect2.value
                    };
                }
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
    }, //tab2: end  
    */
    {
        id: "reviewTab",
        name: "Review",
        columnCount: 1,	  
        sections: [
            /*
          //review section
          {            
            //name: "Service request details",
            type: "review",
            reviewSectionId: "tabs.tab1.sections.sec1",
            controls: [
		      {
			     //id: "usersDropDown1",
                    reviewControlId: "usersDropDown1",
			     //label: "Selected User",  						       
			     valueBinding: function(m, log) {                        				    
                        return [this.value, this.secondaryValue]; //this - referenced ProxyControl
			     }
		      }]
	    }*/
         ]        
         
         }//tab(review)

  ]//tabs
}'

SET IDENTITY_INSERT [ExtendedCaseForms] ON;


DECLARE @caseDataId int
SET @caseDataId = ISNULL((SELECT TOP 1 Id from [dbo].[ExtendedCaseData] where [ExtendedCaseFormId] = 2),0)

--delete child records
IF (@caseDataId > 0)
BEGIN 
     DELETE FROM dbo.[ExtendedCaseValues] where ExtendedCaseDataId = @caseDataId
     DELETE FROM [dbo].[ExtendedCaseData] where Id = @caseDataId
END


DELETE FROM [dbo].[ExtendedCaseForms] WHERE Id = 2

INSERT INTO [dbo].[ExtendedCaseForms]
           (Id
             ,[MetaData]
           ,[Description]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           (2
             ,@metaData
           ,'load save form data test template'
           ,GETDATE()
           ,'test'
           ,GETDATE()
           ,'test')

SET IDENTITY_INSERT [ExtendedCaseForms] OFF;
GO
/*
/*=======================================================================
  == ExtendedCaseData & ExtendedCaseValues test data
  =======================================================================*/
DECLARE	@uniqueId uniqueidentifier,
          @newCaseDataId int

SET @uniqueId = '11B2EE2C-79F1-4F13-9F00-71572702E5CE'

SET @newCaseDataId = ISNULL((SELECT TOP 1 Id from [dbo].[ExtendedCaseData] where [ExtendedCaseFormId] = 2),0)

IF (@newCaseDataId > 0)
BEGIN 
     DELETE FROM dbo.[ExtendedCaseValues] where ExtendedCaseDataId = @newCaseDataId
     DELETE FROM [dbo].[ExtendedCaseData] where Id = @newCaseDataId
END

--insert into ExtendedCaseData
INSERT INTO [dbo].[ExtendedCaseData](ExtendedCaseGuid, ExtendedCaseFormId, CreatedOn, CreatedBy)
VALUES (@uniqueId, 2, GETDATE(), 'me')

SET @newCaseDataId = scope_identity()

INSERT INTO [dbo].[ExtendedCaseValues] (ExtendedCaseDataId, FieldId, Value, SecondaryValue)
VALUES(@newCaseDataId, 'tabs.tab1.sections.sec1.fields.id1', 'hello', null),
       (@newCaseDataId, 'tabs.tab1.sections.sec1.fields.usersDropDown1', '8', 'Per'),
       (@newCaseDataId, 'tabs.tab1.sections.sec1.fields.chk1', 'val1,val2', null)

       
--[dbo].[ExtendedCaseValues]
*/