
/*===========================================================================================
  == Options control data sources data
  ===========================================================================================*/
DELETE FROM [ExtendedCaseOptionDataSources]

SET IDENTITY_INSERT [ExtendedCaseOptionDataSources] ON

INSERT INTO [ExtendedCaseOptionDataSources]([Id],[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES(1,'Regions','sample dt-table','{
				  Type: "db-table",
				  TableName: "tblDepartment",
				  IdColumn: "Id",
				  ValueColumn: "Department",
				  Params: [{
					Name: "regionId",
					ColumnName: "Region_Id"
				  }],
				  ConstParams: [{
					Value: 1,
					ColumnName: "Customer_Id"
				  }]
				}',CAST('20170323 12:49:53.460' as DATETIME),'Me',NULL,NULL)
INSERT INTO [ExtendedCaseOptionDataSources]([Id],[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES(2,'Regions-query','sample db-query','{
	Type: "db-query",
	Query: "select Id as Value, Department as Text from tblDepartment where Customer_id = 1 and Region_Id = @regionId"
}',CAST('20170323 20:05:20.380' as DATETIME),'Me',NULL,NULL)
INSERT INTO [ExtendedCaseOptionDataSources]([Id],[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES(3,'Regions-sp','sample db-sp','{
				  Type: "db-sp",
				  ProcedureName: "uspRegion"
				}',CAST('20170323 20:19:15.713' as DATETIME),'Me',NULL,NULL)
INSERT INTO [ExtendedCaseOptionDataSources]([Id],[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES(7,'Regions-get-query','fill regions','{
	Type: "db-query",
	Query: "select Id as Value, Region as Text from tblRegion where Customer_id = 1"
}',CAST('20170405 00:00:00.000' as DATETIME),'Me',NULL,NULL)
INSERT INTO [ExtendedCaseOptionDataSources]([Id],[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES(8,'Search-query','search dt-query','{
				  Type: "db-query",
				  Query: "select Id as Value, Department as Text from tblDepartment where Customer_id = 1 and Department like @value + ''%''"
				}',CAST('20170323 12:49:53.460' as DATETIME),'Me',NULL,NULL)

SET IDENTITY_INSERT [ExtendedCaseOptionDataSources] OFF
GO

/*===========================================================================================
  == Custom (global) data sources data
  ===========================================================================================*/

DELETE FROM [ExtendedCaseCustomDataSources]
GO

SET IDENTITY_INSERT [ExtendedCaseCustomDataSources] ON
GO

--select admin users for test
INSERT INTO ExtendedCaseCustomDataSources(Id, [DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES (1,
        'getAvaialableUsers',
        'sample db-query',
		'{
			Type: "db-query",
			Query: "SELECT TOP 10 Id, UserId, FirstName, SurName, Email FROM tblUsers WHERE UserGroup_Id IN (3,4) AND Status = 1"
		}',
		GETDATE(),
		'Me',
		NULL,
		NULL)

--select customers for selected user
INSERT INTO ExtendedCaseCustomDataSources(Id, [DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES (2, 
		'customersByUserQuery',
		'sample db-query',
		'{
			Type:  "db-query",
			Query: "select cus.Id, cus.CustomerID, cus.Name, Phone, HelpdeskEmail 
					from tblCustomer cus
						INNER JOIN tblCustomerUser cus_u ON cus_u.[Customer_Id] = cus.Id
					where User_Id = @userId
					ORDER BY cus.Id"

		}',
		GETDATE(),
		'Me',
		NULL,
		NULL)

-- select departments for selected customer
INSERT INTO ExtendedCaseCustomDataSources(Id, [DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES (3,
		'customerDepartmentsQueryDs',
		'sample db-query',
		'{
			Type: "db-query",
			Query: "select Customer_Id, Department, DepartmentId from tblDepartment where Customer_Id = @customerId"
		}',
		GETDATE(),
		'Me',
		NULL,
		NULL)

--table test
INSERT INTO [dbo].[ExtendedCaseCustomDataSources](Id, [DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES (4, 
		'customerDepartmentsTable', --id
		'sample dt-table',
		'{
			Type: "db-table",
			TableName: "tblDepartment",
			Columns: ["Id", "Department", "DepartmentId"],
			Params: [{
				Name: "regionId",
				ColumnName: "Region_Id"
			}],
			ConstParams: [{
			Value: 1,
			ColumnName: "Customer_Id"
			}]
		}',
		GETDATE(),
		'Me',
		NULL,
		NULL)

-- sp test
INSERT INTO [dbo].[ExtendedCaseCustomDataSources](Id,[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES (5,  
		'customerDepartmentsSP',
		'sample dt-sp',
		'{
			Type: "db-sp",
			ProcedureName: "sp_GetCustomerDepartments"		
		 }',
		GETDATE(),
		'Me',
		NULL,
		NULL)
GO

INSERT INTO [dbo].[ExtendedCaseCustomDataSources](Id,[DataSourceId],[Description],[MetaData],[CreatedOn],[CreatedBy],[UpdatedOn],[UpdatedBy])
VALUES (6,  
		'departmentsByUserQuery',
		'sample db-query',
		'{
			Type: "db-query",
			Query: "select Id as Value, Department as Text from tblDepartment where Customer_id = 1 and Department like @value + ''%''"		
		 }',
		GETDATE(),
		'Me',
		NULL,
		NULL)
GO

SET IDENTITY_INSERT [ExtendedCaseCustomDataSources] OFF
GO
/*===========================================================================================*/