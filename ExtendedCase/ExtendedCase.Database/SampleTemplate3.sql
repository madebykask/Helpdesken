DECLARE @metaData nvarchar(MAX) = 
'{
     id: 3,
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
				onSave: [{ type: "required" }	]
				}
               },

               {			
                   id: "txtStatus",
                   type: "textbox",
                   label: "Status",
                   caseBinding: "caseStatus",
                   valueBinding: function(m, log) {
                        //debugger;
                        if (this.pristine){
                            return "standart"
                        }

                        return this.value;
                   }
               },

              {			
                   id: "txtRadio",
                   type: "radio",
                   label: "Confirmation",                   
                   dataSource: [{value: "1", text: "Yes"}, {value: "0", text: "No"}]
              },
               
               //select email of the selected customer from the loaded datasource
               {
                    id: "txtSelectedCustomerEmail",
                    type: "textbox",
                    label: "Customer Email:",

                    validators:{
				    onSave: [{ type: "required" }	]
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


     {
        id: "reviewTab",
        name: "Review",
        columnCount: 1,	  
        sections: [      
        /*
        // orginal review section
        {  
            id: "reviewSection",
            name:"test original review",            
            controls: [
                {	
                    id:"multiselectReview",
                    type:"review",           
                    label:"Multiselect",         
			     valueBinding: function(m, log) {								                    
				    var ctrl = m.tabs.tab1.sections.sec1.first.controls.multiselect1;
                        return [ctrl.value, ctrl.value === ctrl.secondaryValue ? "" : ctrl.secondaryValue];				    
				}
		      },
		      {	
                    id:"userReviewCtrl",
                    type:"review",           
                    label:"Selected User",         
			     valueBinding: function(m, log) {								                    
				    var ctrl = m.tabs.tab1.sections.sec1.first.controls.usersDropDown1;
                        return [ctrl.value, ctrl.value === ctrl.secondaryValue ? "" : ctrl.secondaryValue];				    
				}
		      }]
	    },
        */

         /*
         //new review section test1 
         {  
            type: "review",
            reviewSectionId: "tabs.tab1.sections.sec1",
            controls: [
		      {			     
                    reviewControlId: "usersDropDown1",			     
			        valueBinding: function(m, log) {                        				    
                        return [this.value, "test"]; 
			     }
		      }]
	    },
        */
         /*
         //new review section test2 
         {                        
            type: "review",
            reviewSectionId: "tabs.tab1.sections.sec1",
            reviewControls: "*",
            controls: [
		      {			     
                    reviewControlId: "usersDropDown1",			     
			     valueBinding: function(m, log) {                        				    
                        return [this.value, "test"]; 
			     }
		      }]
	    },*/
         //new review section test3
         {                        
            type: "review",
            reviewSectionId: "tabs.tab1.sections.sec1",            
            controls: [
               {			     
                    reviewControlId: "usersDropDown1",			     
			        valueBinding: function(m, log) {                        				    
                        return [this.value, ""]; 
			     }
		      },
		      {			     
                    reviewControlId: "id1",			     
			        valueBinding: function(m, log) {                        				    
                        return [this.value, ""]; 
			     }
		      }]
	    }
     ]}//tab(review)
  ]//tabs
}'

SET IDENTITY_INSERT [ExtendedCaseForms] ON;


DECLARE @caseDataId int
SET @caseDataId = ISNULL((SELECT TOP 1 Id from [dbo].[ExtendedCaseData] where [ExtendedCaseFormId] = 3),0)

--delete child records
IF (@caseDataId > 0)
BEGIN 
     DELETE FROM dbo.[ExtendedCaseValues] where ExtendedCaseDataId = @caseDataId
     DELETE FROM [dbo].[ExtendedCaseData] where Id = @caseDataId
END


DELETE FROM [dbo].[ExtendedCaseForms] WHERE Id = 3

INSERT INTO [dbo].[ExtendedCaseForms]
           (Id
             ,[MetaData]
           ,[Description]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           (3
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