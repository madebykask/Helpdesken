EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO
DELETE FROM [dbo].[ExtendedCaseForms] WHERE id=11

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
           (11, N'{
				id: 1,
				name: "@Translation.Name",
				localization: {
					dateFormat: "DD-MM-YYYY", // see https://momentjs.com/docs/#/parsing/string-format/ for available formats
					decimalSeparator: ","
				},
				globalFunctions: {},
				/*styles: ".test { color: blue; } ec-section .col-md-6:first-child, ec-review-section .col-md-6:first-child { text-align: left; } ec-section .col-md-6:first-child {width: 60%;}",*/
				styles: ".test { color: red; }\
				 ec-section .col-md-6:first-child, ec-review-section .col-md-6:first-child { text-align: left; }\
				 ec-section .col-md-6:first-child { width: 60%; }\
				 ec-section .col-md-6:nth-child(2) { width: 40%; }\
				 ec-section[name=sec1] h3 { color: blue; }\
				 tab[name=tab1] { font-size: 250%;}\
				 ec-section[name=sec1] h3 { color: red; }",
				dataSources: [
					{
						type: "query",			
						id: "getAvaialableUsers"			
					},
				],
				validatorsMessages: {
					required: "Field is required.",
					alternativeRequired: "Field is required(alternative).",
					dateFormat: "Incorrect format.Expect DD-MM-YYYY",
					percentageMsg: "Should be 1-100"
				},
				tabs: [{
							id: "tab1",
							name: "Html controls",
							columnCount: 1,
							sections: [{						
							id: "sec1",
							name: "Section 1",
							enableAction: {               
								initialState: false,
								label: "Edit"
							},							
							controls: [
							{
								id: "id03",
								type: "textbox",
								htmlLabel: "<b>Html Label</b> By default previous renderer for label will be used. To render html - enable html support for label in template.",
								hiddenBinding: function() {
									return !this.parent.forceEnable;
								},
								validators: {
									onSave: [
										{
											type: "required"
										}]
								}
							},
							/*{
								id: "id04",
								type: "textbox",
								label: "Ordinary Label",
								hiddenBinding: function() {
									return !this.parent.forceEnable;
								}
							},*/
							{
								id: "usersDropDown1",
								type: "dropdown",
								label: "@Translation.SelectUser",
								dataSource: {
									type: "custom",
									id: "getAvaialableUsers",
									valueField: "Id",
									textField: "FirstName"
								},
								hiddenBinding: function() {
									return !this.parent.forceEnable;
								},
								validators: {
									onSave: [
										{
											type: "required"
										}]
								}								
							},
							{
								id: "html1",
								type: "html",
								valueBinding: function(m) {					
									return "<div><b class=test>@Translation.Hi " +(this.parent.controls.usersDropDown1.secondaryValue || "") +"</b> \
											<br>Image:<img src=sun.png alt=sun width=100 height=100><br>\
											Link: <a href=http://www.google.com target=_blank>link to Google</a><br>Table:<br>\
											<table width=100%><tr><th>Name</th><th colspan=2>Telephone</th></tr><tr><td>Bill Gates</td><td>55577854</td><td>55577855</td></tr></table></div>";
								},
								hiddenBinding: function() {
									return !this.parent.forceEnable;
								}
							},
							{
								id: "id18",
								type: "radio",
								label: "Select option",
								dataSource: [{value: "red", text: "Red"}, {value: "black", text: "Black"}],
								hiddenBinding: function() {
									return !this.parent.forceEnable;
								}
							},
							{
								id: "html2",
								type: "html",
								valueBinding: function(m) {					
									return "<div style=color:" +m.tabs.tab1.sections.sec1.controls.id18.value +">\
									Halmstad, at the time part of the Kingdom of Denmark, received its first city charter in 1307, and the city celebrated its 700th anniversary in 2007. The oldest remains of that first town are to be found at &quot;Övraby&quot; upstream on Nissan, just south of and quite close to the present day regiment buildings. The remains of the church can still be seen today between a defunct brick industry and a former landfill.<br>\
									<br>In the 1320s the town moved to the present day town centre. At this time there were two monasteries in the town and during the 15th century the St. Nikolai church was built. Halland was the object of numerous battles, sieges and occupations by Swedish troops.</div>";
								},
								hiddenBinding: function(m) {
									return !this.parent.forceEnable || (this.parent.forceEnable && this.parent.controls.id18.value === "");
								}
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