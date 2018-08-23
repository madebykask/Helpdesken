USE [msdb]
GO

/****** Object:  Job [RunSP_CBD_ComputerUser_Sync]    Script Date: 08/22/2018 17:55:38 ******/
IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'RunSP_CBD_ComputerUser_Sync')
EXEC msdb.dbo.sp_delete_job @job_id=N'7d564e59-fb2e-4e79-9e33-ca03baa7d315', @delete_unused_schedule=1
GO

USE [msdb]
GO

/****** Object:  Job [RunSP_CBD_ComputerUser_Sync]    Script Date: 08/22/2018 17:55:38 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 08/22/2018 17:55:38 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'RunSP_CBD_ComputerUser_Sync', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Execute SP]    Script Date: 08/22/2018 17:55:38 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Execute SP', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @customerID INT = 1,
    	  @ouID INT = NULL,
    	  @computerUserCategoryGUID UNIQUEIDENTIFIER = ''9DB68A9D-F93D-45DB-8940-102869A9A870''
 
DECLARE    @return_value int
 
EXEC    	@return_value = [dbo].[CBD_ComputerUser_Sync]
        	@customerID,
        	@ouID,
        	@computerUserCategoryGUID
 
SELECT    ''Return Value'' = @return_value
 
GO', 
		@database_name=N'DH_Support', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'CBD_ComputerUser_Sync', 
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
