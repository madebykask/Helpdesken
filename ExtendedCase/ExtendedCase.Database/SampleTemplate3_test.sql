DECLARE @metaData nvarchar(MAX) = 
'{
    id: 3,
    name: "@Translation.Name",
    localization: {
        dateFormat: "DD-MM-YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
        decimalSeparator: ","
    },
    dataSources: [],
    tabs: [{
            id: "tab1",
            name: "Tab 1",
            sections: [{
                    id: "testSec",
                    name: "Section Test",
                    controls: [{
                        id: "toggleCtl",
                        type: "radio",
                        label: "Confirmation",
                        dataSource: [{
                            value: "1",
                            text: "Yes"
                        }, {
                            value: "0",
                            text: "No"
                        }],
                    }]
                },
                {
                    id: "testSec2",
                    name: "SectionTest2",
                    hiddenBinding: function (m, l) {
                        //debugger;
                        return m.tabs.tab1.sections.testSec.controls.toggleCtl.value == "0" ||
                            m.tabs.tab1.sections.testSec.controls.toggleCtl.value == "";
                    },
                    controls: [{
                        id: "email",
                        type: "textbox",
                        label: "Email",
                        valueBinding: function () {
                            return "test@ya.ru"
                        }
                    }]
                }
            ]
        },
        {
            id: "reviewTab",
            name: "Review",
            columnCount: 1,
            sections: [
                //new review section test2 
                {
                    type: "review",
                    reviewSectionId: "tabs.tab1.sections.testSec2",
                    reviewControls: "*"
                }
            ]
        } //tab(review)
    ] //tabs
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