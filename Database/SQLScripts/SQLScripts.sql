
--update DB from 5.3.33 to 5.3.34 version

	TRUNCATE TABLE [dbo].[tblCaseSolutionConditionProperties]

	declare @sortOrder int = 0

	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder])
     VALUES
           ('case_StateSecondary.StateSecondaryGUID',
			'Ärende - Understatus',
			'tblStateSecondary',
			'Id',
			'StateSecondary',
			'StateSecondaryGUID',
			'Status',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder])
     VALUES
           ('case_WorkingGroup.WorkingGroupGUID',
			'Ärende - Driftgrupp',
			'tblWorkingGroup',
			'Id',
			'WorkingGroup',
			'WorkingGroupGUID',
			'Status',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder])
     VALUES
           ('case_Priority.PriorityGUID',
			'Ärende - Prioritet',
			'tblPriority',
			'Id',
			'PriorityName',
			'PriorityGUID',
			'Status',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder])
		   
     VALUES
           ('case_Status.StatusGUID',
			'Ärende - Status',
			'tblStatus',
			'Id',
			'StatusName',
			'StatusGUID',
			'Status',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder]
		   ,[TableParentId])
     VALUES
           ('case_ProductArea.ProductAreaGUID',
			'Ärende - Produktområde',
			'tblProductArea',
			'Id',
			'ProductArea',
			'ProductAreaGUID',
			'Status',
			@sortOrder
			,'Parent_ProductArea_Id')

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('case_Relation',
			'Kopplat ärende',
			'tblYesNo',
			'Id',
			'Value',
			'Id',
			@sortOrder)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder])
     VALUES
           ('user_WorkingGroup.WorkingGroupGUID',
			'Användare - Driftgrupp',
			'tblWorkingGroup',
			'Id',
			'WorkingGroup',
			'WorkingGroupGUID',
			'Status',
			@sortOrder
			)

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[TableFieldStatus]
		   ,[SortOrder]
		   ,[TableParentId])
     VALUES
           ('casesolution_ProductArea.ProductAreaGUID',
			'Ärendemall - Produktområde',
			'tblProductArea',
			'Id',
			'ProductArea',
			'ProductAreaGUID',
			'Status',
			@sortOrder,
			'Parent_ProductArea_Id')

	set @sortOrder = @sortOrder+1
	INSERT INTO [dbo].[tblCaseSolutionConditionProperties]
           ([CaseSolutionConditionProperty]
           ,[Text]
           ,[Table]
           ,[TableFieldId]
           ,[TableFieldName]
           ,[TableFieldGuid]
		   ,[SortOrder])
     VALUES
           ('application_type',
			'Applikation',
			'tblApplicationType',
			'Id',
			'ApplicationType',
			'Id',
			@sortOrder)






-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.34'