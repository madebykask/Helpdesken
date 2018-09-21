USE [msdb]
GO


DECLARE @jobName nvarchar(50)
SET @jobName = N'CBD2Helpdesk sync'

IF EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = @jobName)
    EXEC msdb.dbo.sp_delete_job @job_name=@jobName, @delete_unused_schedule=1
GO

USE [msdb]
GO
BEGIN TRANSACTION
GO

    DECLARE @jobOwnerLogin nvarchar(50),		  
		  @helpdeskDbUser nvarchar(50),
		  @helpdeskDatabaseName nvarchar(50),
		  @computerUserCategoryGUID nvarchar(50),
		  @customerId int

    ----------------------------------------------------------------------------------------------------
    -- INPUT PARAMETERS - Environment/Customer specific values to be changed before running a script!!!
    ----------------------------------------------------------------------------------------------------
    
    -- Helpdesk database name 
    SET @helpdeskDatabaseName = N'DH_Support'

    -- UniqueId (tblComputerUsersCategory.ComputerUsersCategoryGuid) of the vendors computer user category where data will be synced. 
    SET @computerUserCategoryGUID = 'AD565179-E89D-41C0-8176-2A5B23FFD402' -- Check if it exists or find other vendors category

    -- Change to the required customer id from tblCustomers
    SET @customerId = 1 
    
    -- Note: The name of the login that owns the job. 
    --       login is sysname, with a default of NULL, which is interpreted as the current login name. 
    --       Only members of the sysadmin fixed server role can set or change the value for @owner_login_name. 
    --      If users who are not members of the sysadmin role set or change the value of @owner_login_name, execution of sp_add_job stored procedure fails and an error is returned.
    SET @jobOwnerLogin = NULL  -- REPLACE WITH YOUR LOGIN

    -- NOTE: The name of the user account to use when executing a Transact-SQL job step. 
    --       user is sysname, with a default of NULL. When user is NULL, the step runs in the job owner's user context on database. 
    --       SQL Server Agent will include this parameter only if the job owner is a SQL Server sysadmin. 
    --       If so, the given Transact-SQL step will be executed in the context of the given SQL Server user name. 
    --       If the job owner is not a SQL Server sysadmin, then the Transact-SQL step will always be executed in the context of the login that owns this job, 
    --       and the @database_user_name parameter will be ignored.
    SET @helpdeskDbUser = NULL -- Helpdesk db user name 
    
    --------------------------------------------------------    
    
    DECLARE @jobCategory nvarchar(256),
		  @jobName nvarchar(50)
    
    -- CAN BE CHANGED 
    SET @jobCategory = N'Helpdesk Jobs'
    SET @jobName     = N'CBD2Helpdesk sync' -- if you change remember to update drop script at the top 

    DECLARE @ReturnCode INT
    SELECT @ReturnCode = 0

    -- 1. Create JobCategory 
    IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=@jobCategory AND category_class=1)
    BEGIN
	   EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name = @jobCategory
	   IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
    END
        
    DECLARE @jobId BINARY(16)
    
    -- 2. Add job: https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-add-job-transact-sql?view=sql-server-2017&viewFallbackFrom=sql-server-2014
    EXEC @ReturnCode = msdb.dbo.sp_add_job 
		@job_name=@jobName, 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 		
		@category_name=@jobCategory, 
		@owner_login_name= @jobOwnerLogin,
		@description=N'Job to sync data from CBD tables to tblComputerUsers Helpdesk table', 
		@job_id = @jobId OUTPUT

    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

    DECLARE @cmd nvarchar(MAX)
    SET @cmd = N'
  DECLARE @customerID INT = ' + CONVERT(nvarchar(10), @customerId) + ',
    		@ouID INT = NULL,
    	     @computerUserCategoryGUID UNIQUEIDENTIFIER = ''' + @computerUserCategoryGUID + '''
 
  DECLARE @return_value int
 
    EXEC  @return_value = [dbo].[CBD_ComputerUser_Sync]
        	@customerID,
        	@ouID,
        	@computerUserCategoryGUID
 
SELECT    ''Return Value'' = @return_value
GO'


    -- 3. Add Job step: https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-add-jobstep-transact-sql?view=sql-server-2017&viewFallbackFrom=sql-server-2014
    EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Run sync SP', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, 
		@subsystem=N'TSQL', 
		@command= @cmd, 
		@database_name = @helpdeskDatabaseName, 
		@database_user_name = @helpdeskDbUser, 
		@flags=0

    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

    EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1

    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

    EXEC  @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'CBD_ComputerUser_Sync', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20180627, 
		@active_end_date=99991231, 
		@active_start_time=184800, 
		@active_end_time=235959, 
		@schedule_uid=N'f86c95f2-fcf0-420c-a143-46fc4c9df2f0'

    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback


    EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

COMMIT TRANSACTION

GOTO EndSave

QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO
