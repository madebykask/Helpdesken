--update DB from 5.3.58.0 to 5.7.3 version

-- 5.3.58.0 --

--Altered these column to reflect the sizes in tblCase
--tblCaseIsAbout
RAISERROR ('Alter tblCaseIsAbout.ReportedBy from 40 nvarchar to 200 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseIsAbout','ReportedBy') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseIsAbout]
		ALTER COLUMN ReportedBy nvarchar(200) NULL
	End
Go

RAISERROR ('Alter tblCaseIsAbout.Person_Phone from 40 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseIsAbout','Person_Phone') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseIsAbout]
		ALTER COLUMN Person_Phone nvarchar(50) NULL
	End
Go

RAISERROR ('Alter tblCaseIsAbout.Person_CellPhone from 30 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseIsAbout','Person_CellPhone') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseIsAbout]
		ALTER COLUMN Person_CellPhone nvarchar(50) NULL
	End
Go

--tblCaseHistory 

RAISERROR ('Alter tblCaseHistory.IsAbout_ReportedBy from 40 nvarchar to 200 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseHistory','IsAbout_ReportedBy') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseHistory]
		ALTER COLUMN IsAbout_ReportedBy nvarchar(200) NULL
	End
Go

RAISERROR ('Alter tblCaseHistory.IsAbout_Persons_Phone from 40 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseHistory','IsAbout_Persons_Phone') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseHistory]
		ALTER COLUMN IsAbout_Persons_Phone nvarchar(50) NULL
	End
Go

RAISERROR ('Alter Persons_Phone and Persons_CellPhone in ECT_Get_MailById from 40/30 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[ECT_Get_MailById]', 'P') IS NOT NULL) 
DROP PROCEDURE [dbo].[ECT_Get_MailById]
GO

CREATE PROCEDURE [dbo].[ECT_Get_MailById]  
 (  
  @MailTemplate_Id int,  
  @Case_Id int,  
  @Language_Id int  
 )  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 DECLARE @MailId int  
 DECLARE @Customer_Id int  
 DECLARE @MailTemplateSubject nvarchar(200)  
    DECLARE @MailTemplateBody nvarchar(4000)  
      
 DECLARE @ReportedBy nvarchar(200)  
 DECLARE @Persons_Name nvarchar(50)  
 DECLARE @Persons_EMail nvarchar(100)  
 DECLARE @Persons_Phone nvarchar(50)  
 DECLARE @Persons_CellPhone nvarchar(50)  
  
 DECLARE @CaseNumber nvarchar(20)  
 DECLARE @RegTime nvarchar(20)  
 DECLARE @ChangedBy nvarchar(100)  
 DECLARE @CustomerName nvarchar(50)  
 DECLARE @Place nvarchar(100)  
 DECLARE @InventoryNumber nvarchar(20)  
 DECLARE @CaseType nvarchar(50)  
 DECLARE @Category nvarchar(50)  
 DECLARE @Caption nvarchar(60)  
 DECLARE @Description nvarchar(1000)  
 DECLARE @Miscellaneous nvarchar(1000)  
 DECLARE @Available nvarchar(100)  
 DECLARE @WorkingGroup nvarchar(50)  
 DECLARE @WorkingGroupEMail nvarchar(200)  
 DECLARE @AdministratorFirstName nvarchar(20)  
 DECLARE @AdministratorSurName nvarchar(30)  
 DECLARE @PriorityName nvarchar(30)  
 DECLARE @PriorityDescription nvarchar(200)  
 DECLARE @WatchDate nvarchar(20)  
  
 SELECT @MailId=MailId, @Customer_Id=Customer_Id, @MailTemplateSubject=Subject, @MailTemplateBody=Body   
 FROM tblMailTemplate  
  INNER JOIN tblMailTemplate_tblLanguage ON tblMailTemplate.Id=tblMailTemplate_tblLanguage.MailTemplate_Id  
 WHERE MailTemplate_Id=@MailTemplate_Id AND Language_Id=@Language_Id  
  
 SELECT   
  @ReportedBy=tblCase.ReportedBy,  
  @Persons_Name=tblCase.Persons_Name,  
  @Persons_EMail=tblCase.Persons_EMail,  
  @Persons_Phone=tblCase.Persons_Phone,  
  @Persons_CellPhone=tblCase.Persons_CellPhone,   
  @Place=tblCase.Place,   
  @CaseNumber=tblCase.CaseNumber,  
  @RegTime=tblCase.RegTime,  
  @ChangedBy=ISNULL(tblUsers.FirstName, '') + ' ' + ISNULL(tblUsers.SurName, ''),  
  @CustomerName=tblCustomer.Name,  
  @InventoryNumber=tblCase.InventoryNumber,  
  @CaseType=tblCaseType.CaseType,  
  @Category=ISNULL(tblCategory.Category, ''),  
  @Caption=tblCase.Caption,  
  @Description=tblCase.Description,  
  @Miscellaneous=tblCase.Miscellaneous,  
  @Available=tblCase.Available,  
  @WorkingGroup=ISNULL(tblWorkingGroup.WorkingGroup, ''),  
  @WorkingGroupEMail=ISNULL(tblWorkingGroup.WorkingGroupEMail, ''),  
  @AdministratorFirstName=ISNULL(tblUsers2.FirstName, ''),  
  @AdministratorSurName=ISNULL(tblUsers2.SurName, ''),  
  @PriorityName=ISNULL(tblPriority.PriorityName, ''),  
  @PriorityDescription=ISNULL(tblPriority.PriorityDescription, ''),  
  @WatchDate=ISNULL(tblCase.WatchDate, '')  
 FROM tblCase  
  INNER JOIN tblCustomer ON tblCase.Customer_Id=tblCustomer.Id  
  LEFT JOIN tblUsers ON tblCase.ChangeByUser_Id = tblUsers.Id  
  LEFT JOIN tblUsers tblUsers2 ON tblCase.Performer_User_Id = tblUsers2.Id  
  INNER JOIN tblCaseType ON tblCase.Casetype_Id=tblCaseType.Id  
  LEFT JOIN tblCategory ON tblCase.Category_Id = tblCategory.Id  
  LEFT JOIN tblWorkingGroup ON tblCase.WorkingGroup_Id = tblWorkingGroup.Id  
  LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id  
 WHERE tblCase.Id=@Case_Id  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#1]',  @Casenumber)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#1]',  @Casenumber)  
   
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#16]',  @RegTime)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#16]',  @RegTime)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#22]',  @ChangedBy)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#22]',  @ChangedBy)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#27]',  @ReportedBy)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#27]',  @ReportedBy)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#3]',  @Persons_Name)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#3]',  @Persons_Name)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#8]',  @Persons_EMail)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#8]',  @Persons_EMail)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#9]',  @Persons_Phone)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#9]',  @Persons_Phone)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#18]',  @Persons_CellPhone)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#18]',  @Persons_CellPhone)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#2]',  @CustomerName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#2]',  @CustomerName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#24]',  @Place)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#24]',  @Place)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#17]',  @InventoryNumber)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#17]',  @InventoryNumber)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#25]',  @CaseType)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#25]',  @CaseType)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#4]',  @Caption)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#4]',  @Caption)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#5]',  @Description)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#5]',  @Description)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#23]',  @Miscellaneous)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#23]',  @Miscellaneous)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#19]',  @Available)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#19]',  @Available)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#15]',  @WorkingGroup)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#15]',  @WorkingGroup)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#13]',  @WorkingGroupEMail)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#13]',  @WorkingGroupEMail)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#6]',  @AdministratorFirstName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#6]',  @AdministratorFirstName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#7]',  @AdministratorSurName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#7]',  @AdministratorSurName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#12]',  @PriorityName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#12]',  @PriorityName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#20]',  @PriorityDescription)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#20]',  @PriorityDescription)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#21]',  @WatchDate)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#21]',  @WatchDate)  
  
 SELECT @MailId AS MailId, @Customer_Id AS Customer_Id, @MailTemplate_Id AS Id, @MailTemplateSubject AS Subject, @MailTemplateBody AS Body  
END  
GO

RAISERROR ('Alter Persons_Phone and Persons_CellPhone in ECT_Get_MailByMailId from 40/30 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[ECT_Get_MailByMailId]', 'P') IS NOT NULL) 
DROP PROCEDURE [dbo].[ECT_Get_MailByMailId]
GO

CREATE PROCEDURE [dbo].[ECT_Get_MailByMailId]  
 (  
  @MailId int,  
  @Customer_Id int,  
  @Language_Id int,  
  @Case_Id int  
 )  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 DECLARE @MailTemplate_Id int  
 DECLARE @MailTemplateSubject nvarchar(200)  
    DECLARE @MailTemplateBody nvarchar(4000)  
      
 DECLARE @ReportedBy nvarchar(200)  
 DECLARE @Persons_Name nvarchar(50)  
 DECLARE @Persons_EMail nvarchar(100)  
 DECLARE @Persons_Phone nvarchar(50)  
 DECLARE @Persons_CellPhone nvarchar(50)  
  
 DECLARE @CaseGUID uniqueidentifier  
 DECLARE @CaseNumber nvarchar(20)  
 DECLARE @RegTime nvarchar(20)  
 DECLARE @ChangedBy nvarchar(100)  
 DECLARE @CustomerName nvarchar(50)  
 DECLARE @Place nvarchar(100)  
 DECLARE @InventoryNumber nvarchar(20)  
 DECLARE @CaseType nvarchar(50)  
 DECLARE @Category nvarchar(50)  
 DECLARE @Caption nvarchar(60)  
 DECLARE @Description nvarchar(1000)  
 DECLARE @Miscellaneous nvarchar(1000)  
 DECLARE @Available nvarchar(100)  
 DECLARE @WorkingGroup nvarchar(50)  
 DECLARE @WorkingGroupEMail nvarchar(200)  
 DECLARE @AdministratorFirstName nvarchar(20)  
 DECLARE @AdministratorSurName nvarchar(30)  
 DECLARE @PriorityName nvarchar(30)  
 DECLARE @PriorityDescription nvarchar(200)  
 DECLARE @WatchDate nvarchar(20)  
 DECLARE @SiteURL nvarchar(100)  
 DECLARE @SelfServiceURL nvarchar(100)  
 DECLARE @Version int  
 DECLARE @Link nvarchar(500)  
 DECLARE @LinkSelfService nvarchar(500)  
 DECLARE @ServerPort int  
 DECLARE @Protocol nvarchar(10)  
  
 SELECT @ServerPort = ServerPort FROM tblGlobalSettings  
 SELECT @SiteURL = SiteURL, @SelfServiceURL = SelfServiceURL FROM tblSettings WHERE Customer_Id=@Customer_Id  
  
 if @SiteURL IS NULL  
  begin  
   SELECT @SiteURL = ServerName FROM tblGlobalSettings  
   SET @Version = 4  
  end  
 else  
  begin  
   SET @Version = 5  
  end  
  
 if @SelfServiceURL IS NULL  
  begin  
   SELECT @SelfServiceURL = ServerName FROM tblGlobalSettings  
  end  
  
 if CHARINDEX(@SiteURL, 'http') = 0  
  begin  
   if @ServerPort = 443  
    SET @SiteURL = 'https://' + @SiteURL  
   else  
    SET @SiteURL = 'http://' + @SiteURL  
  end   
  
 if CHARINDEX(@SelfServiceURL, 'http') = 0  
  begin  
   if @ServerPort = 443  
    SET @SelfServiceURL = 'https://' + @SelfServiceURL  
   else  
    SET @SelfServiceURL = 'http://' + @SelfServiceURL  
  end   
  
 SELECT @MailTemplate_Id=MailTemplate_Id, @MailTemplateSubject=Subject, @MailTemplateBody=Body   
 FROM tblMailTemplate  
  INNER JOIN tblMailTemplate_tblLanguage ON tblMailTemplate.Id=tblMailTemplate_tblLanguage.MailTemplate_Id  
 WHERE MailId=@MailId AND Language_Id=@Language_Id AND Customer_Id=@Customer_Id  
  
 SELECT   
  @CaseGUID=tblCase.CaseGUID,  
  @ReportedBy=ISNULL(tblCase.ReportedBy, ''),  
  @Persons_Name=tblCase.Persons_Name,  
  @Persons_EMail=tblCase.Persons_EMail,  
  @Persons_Phone=tblCase.Persons_Phone,  
  @Persons_CellPhone=tblCase.Persons_CellPhone,   
  @Place=tblCase.Place,   
  @CaseNumber=tblCase.CaseNumber,  
  @RegTime=tblCase.RegTime,  
  @ChangedBy=ISNULL(tblUsers.FirstName, '') + ' ' + ISNULL(tblUsers.SurName, ''),  
  @CustomerName=tblCustomer.Name,  
  @InventoryNumber=tblCase.InventoryNumber,  
  @CaseType=tblCaseType.CaseType,  
  @Category=ISNULL(tblCategory.Category, ''),  
  @Caption=tblCase.Caption,  
  @Description=tblCase.Description,  
  @Miscellaneous=tblCase.Miscellaneous,  
  @Available=tblCase.Available,  
  @WorkingGroup=ISNULL(tblWorkingGroup.WorkingGroup, ''),  
  @WorkingGroupEMail=ISNULL(tblWorkingGroup.WorkingGroupEMail, ''),  
  @AdministratorFirstName=ISNULL(tblUsers2.FirstName, ''),  
  @AdministratorSurName=ISNULL(tblUsers2.SurName, ''),  
  @PriorityName=ISNULL(tblPriority.PriorityName, ''),  
  @PriorityDescription=ISNULL(tblPriority.PriorityDescription, ''),  
  @WatchDate=ISNULL(tblCase.WatchDate, '')  
 FROM tblCase  
  INNER JOIN tblCustomer ON tblCase.Customer_Id=tblCustomer.Id  
  LEFT JOIN tblUsers ON tblCase.ChangeByUser_Id = tblUsers.Id  
  LEFT JOIN tblUsers tblUsers2 ON tblCase.Performer_User_Id = tblUsers2.Id  
  INNER JOIN tblCaseType ON tblCase.Casetype_Id=tblCaseType.Id  
  LEFT JOIN tblCategory ON tblCase.Category_Id = tblCategory.Id  
  LEFT JOIN tblWorkingGroup ON tblCase.WorkingGroup_Id = tblWorkingGroup.Id  
  LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id  
 WHERE tblCase.Id=@Case_Id  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#1]',  @Casenumber)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#1]',  @Casenumber)  
   
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#16]',  @RegTime)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#16]',  @RegTime)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#22]',  @ChangedBy)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#22]',  @ChangedBy)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#27]',  @ReportedBy)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#27]',  @ReportedBy)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#3]',  @Persons_Name)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#3]',  @Persons_Name)  
  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#8]',  @Persons_EMail)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#8]',  @Persons_EMail)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#9]',  @Persons_Phone)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#9]',  @Persons_Phone)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#18]',  @Persons_CellPhone)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#18]',  @Persons_CellPhone)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#2]',  @CustomerName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#2]',  @CustomerName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#24]',  @Place)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#24]',  @Place)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#17]',  @InventoryNumber)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#17]',  @InventoryNumber)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#25]',  @CaseType)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#25]',  @CaseType)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#4]',  @Caption)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#4]',  @Caption)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#5]',  @Description)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#5]',  @Description)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#23]',  @Miscellaneous)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#23]',  @Miscellaneous)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#19]',  @Available)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#19]',  @Available)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#15]',  @WorkingGroup)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#15]',  @WorkingGroup)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#13]',  @WorkingGroupEMail)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#13]',  @WorkingGroupEMail)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#6]',  @AdministratorFirstName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#6]',  @AdministratorFirstName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#7]',  @AdministratorSurName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#7]',  @AdministratorSurName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#12]',  @PriorityName)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#12]',  @PriorityName)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#20]',  @PriorityDescription)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#20]',  @PriorityDescription)  
  
 SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#21]',  @WatchDate)  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#21]',  @WatchDate)  
  
 if @Version = 5  
  SET @Link = '<br><a href="' + @SiteURL + '/cases/edit/' + convert(varchar(50),@Case_Id) + '">' + @SiteURL + '/cases/edit/' + convert(varchar(50),@Case_Id) + '</a>'  
    else  
        SET @Link = '<br><a href="' + @SiteURL + '/Default.asp?GUID=' + convert(varchar(50),@CaseGUID) + '">' + @SiteURL + '/Default.asp?GUID=' + convert(varchar(50),@CaseGUID) + '</a>'  
       
 --print @MailTemplateBody  
  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#99]',  @Link)  
  
    if @Version = 5  
  SET @LinkSelfService = '<br><a href="' + @SelfServiceURL + '/case/index/' + convert(varchar(50),@CaseGUID) + '">' + @SelfServiceURL + '/case/index/' + convert(varchar(50),@CaseGUID) + '</a>'  
    else  
  SET @LinkSelfService = '<br><a href="' + @SelfServiceURL + '/CI.asp?Id=' + convert(varchar(50),@CaseGUID) + '>' + @SelfServiceURL + '/CI.asp?Id=' + convert(varchar(50),@CaseGUID) + '</a>'  
  
 SET @MailTemplateBody = replace(@MailTemplateBody, '[#98]',  @LinkSelfService)  
  
 SELECT @MailId AS MailId, @Customer_Id AS Customer_Id, @MailTemplate_Id AS Id, @MailTemplateSubject AS Subject, @MailTemplateBody AS Body  
END  
GO

RAISERROR ('Alter Persons_Phone and Persons_CellPhone in Mail2License_getMailById from 40/30 nvarchar to 50 nvarchar', 10, 1) WITH NOWAIT
IF(OBJECT_ID('[dbo].[Mail2License_getMailById]', 'P') IS NOT NULL) 
DROP PROCEDURE [dbo].[Mail2License_getMailById]
GO

CREATE PROCEDURE [dbo].[Mail2License_getMailById]  
 (  
  @MailId int,  
  @Customer_Id int,  
  @Language_Id int,  
  @Case_Id int  
 )  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 if @Case_Id = 0   
  begin  
   SELECT tblMailTemplate.Id, tblMailTemplate.MailId, tblMailTemplate_tblLanguage.Subject, tblMailTemplate_tblLanguage.Body  
   FROM tblMailTemplate  
    INNER JOIN tblMailTemplate_tblLanguage ON tblMailTemplate.Id=tblMailTemplate_tblLanguage.MailTemplate_Id AND tblMailTemplate_tblLanguage.Language_Id=@Language_Id  
   WHERE tblMailTemplate.MailId=@MailId AND Language_Id=@Language_Id  
  end  
 else  
  begin  
   DECLARE @MailTemplate_Id int  
   DECLARE @MailTemplateSubject nvarchar(200)  
   DECLARE @MailTemplateBody nvarchar(4000)  
      
   DECLARE @ReportedBy nvarchar(200)  
   DECLARE @Persons_Name nvarchar(50)  
   DECLARE @Persons_EMail nvarchar(100)  
   DECLARE @Persons_Phone nvarchar(50)  
   DECLARE @Persons_CellPhone nvarchar(50)  
  
   DECLARE @CaseNumber nvarchar(20)  
   DECLARE @RegTime nvarchar(20)  
   DECLARE @ChangedBy nvarchar(100)  
   DECLARE @CustomerName nvarchar(50)  
   DECLARE @Place nvarchar(100)  
   DECLARE @InventoryNumber nvarchar(20)  
   DECLARE @CaseType nvarchar(50)  
   DECLARE @Category nvarchar(50)  
   DECLARE @Caption nvarchar(60)  
   DECLARE @Description nvarchar(1000)  
   DECLARE @Miscellaneous nvarchar(1000)  
   DECLARE @Available nvarchar(100)  
   DECLARE @WorkingGroup nvarchar(50)  
   DECLARE @WorkingGroupEMail nvarchar(200)  
   DECLARE @AdministratorFirstName nvarchar(20)  
   DECLARE @AdministratorSurName nvarchar(30)  
   DECLARE @PriorityName nvarchar(30)  
   DECLARE @PriorityDescription nvarchar(200)  
   DECLARE @WatchDate nvarchar(20)  
  
   SELECT @MailTemplate_Id=MailTemplate_Id, @MailTemplateSubject=Subject, @MailTemplateBody=Body   
   FROM tblMailTemplate  
    INNER JOIN tblMailTemplate_tblLanguage ON tblMailTemplate.Id=tblMailTemplate_tblLanguage.MailTemplate_Id  
   WHERE MailId=@MailId AND Language_Id=@Language_Id AND Customer_Id=@Customer_Id  
  
   SELECT   
    @ReportedBy=tblCase.ReportedBy,  
    @Persons_Name=tblCase.Persons_Name,  
    @Persons_EMail=tblCase.Persons_EMail,  
    @Persons_Phone=tblCase.Persons_Phone,  
    @Persons_CellPhone=tblCase.Persons_CellPhone,   
    @Place=tblCase.Place,   
    @CaseNumber=tblCase.CaseNumber,  
    @RegTime=tblCase.RegTime,  
    @ChangedBy=ISNULL(tblUsers.FirstName, '') + ' ' + ISNULL(tblUsers.SurName, ''),  
    @CustomerName=tblCustomer.Name,  
    @InventoryNumber=tblCase.InventoryNumber,  
    @CaseType=tblCaseType.CaseType,  
    @Category=ISNULL(tblCategory.Category, ''),  
    @Caption=tblCase.Caption,  
    @Description=tblCase.Description,  
    @Miscellaneous=tblCase.Miscellaneous,  
    @Available=tblCase.Available,  
    @WorkingGroup=ISNULL(tblWorkingGroup.WorkingGroup, ''),  
    @WorkingGroupEMail=ISNULL(tblWorkingGroup.WorkingGroupEMail, ''),  
    @AdministratorFirstName=ISNULL(tblUsers2.FirstName, ''),  
    @AdministratorSurName=ISNULL(tblUsers2.SurName, ''),  
    @PriorityName=ISNULL(tblPriority.PriorityName, ''),  
    @PriorityDescription=ISNULL(tblPriority.PriorityDescription, ''),  
    @WatchDate=ISNULL(tblCase.WatchDate, '')  
   FROM tblCase  
    INNER JOIN tblCustomer ON tblCase.Customer_Id=tblCustomer.Id  
    LEFT JOIN tblUsers ON tblCase.ChangeByUser_Id = tblUsers.Id  
    LEFT JOIN tblUsers tblUsers2 ON tblCase.Performer_User_Id = tblUsers2.Id  
    INNER JOIN tblCaseType ON tblCase.Casetype_Id=tblCaseType.Id  
    LEFT JOIN tblCategory ON tblCase.Category_Id = tblCategory.Id  
    LEFT JOIN tblWorkingGroup ON tblCase.WorkingGroup_Id = tblWorkingGroup.Id  
    LEFT JOIN tblPriority ON tblCase.Priority_Id = tblPriority.Id  
   WHERE tblCase.Id=@Case_Id  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#1]',  @Casenumber)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#1]',  @Casenumber)  
   
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#16]',  @RegTime)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#16]',  @RegTime)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#22]',  @ChangedBy)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#22]',  @ChangedBy)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#27]',  @ReportedBy)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#27]',  @ReportedBy)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#3]',  @Persons_Name)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#3]',  @Persons_Name)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#8]',  @Persons_EMail)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#8]',  @Persons_EMail)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#9]',  @Persons_Phone)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#9]',  @Persons_Phone)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#18]',  @Persons_CellPhone)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#18]',  @Persons_CellPhone)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#2]',  @CustomerName)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#2]',  @CustomerName)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#24]',  @Place)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#24]',  @Place)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#17]',  @InventoryNumber)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#17]',  @InventoryNumber)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#25]',  @CaseType)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#25]',  @CaseType)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#4]',  @Caption)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#4]',  @Caption)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#5]',  @Description)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#5]',  @Description)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#23]',  @Miscellaneous)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#23]',  @Miscellaneous)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#19]',  @Available)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#19]',  @Available)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#15]',  @WorkingGroup)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#15]',  @WorkingGroup)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#13]',  @WorkingGroupEMail)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#13]',  @WorkingGroupEMail)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#6]',  @AdministratorFirstName)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#6]',  @AdministratorFirstName)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#7]',  @AdministratorSurName)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#7]',  @AdministratorSurName)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#12]',  @PriorityName)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#12]',  @PriorityName)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#20]',  @PriorityDescription)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#20]',  @PriorityDescription)  
  
   SET @MailTemplateSubject = replace(@MailTemplateSubject, '[#21]',  @WatchDate)  
   SET @MailTemplateBody = replace(@MailTemplateBody, '[#21]',  @WatchDate)  
  
   SELECT @MailId AS MailId, @Customer_Id AS Customer_Id, @MailTemplate_Id AS Id, @MailTemplateSubject AS Subject, @MailTemplateBody AS Body  
  end  
END
GO

-- Add scripts to 5.3.58.1 here
RAISERROR ('Add Column IncludeLogText_External to tblMailTemplate', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblMailTemplate','IncludeLogText_External') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblMailTemplate]
		ADD IncludeLogText_External bit not null default 0
	End
Go

RAISERROR ('Alter tblDepartment.SearchKey from 200 nvarchar to 400 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblDepartment','SearchKey') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblDepartment]
		ALTER COLUMN SearchKey nvarchar(400) NULL
	End
Go

--new for EctReports to Sharepoint for Ikea 2023-06-07
RAISERROR ('Add Column SharePointSiteId to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointSiteId') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointSiteId nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointUserName to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointUserName') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointUserName nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointPassword to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointPassword') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointPassword nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointFolderId to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointFolderId') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointFolderId nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointDriveId to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointDriveId') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointDriveId nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointSecretKey to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointSecretKey') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointSecretKey nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointClientId to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointClientId') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointClientId nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointTenantId to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointTenantId') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointTenantId nvarchar(200) null
	End
Go

RAISERROR ('Add Column SharePointScope to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','SharePointScope') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD SharePointScope nvarchar(200) null
	End
Go

-- 5.4.0

RAISERROR ('Add Column AutocloseDays to tblStateSecondary', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblStateSecondary','AutocloseDays') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblStateSecondary]
		ADD AutocloseDays INT DEFAULT 0
	End
Go

ALTER TRIGGER [dbo].[TR_CreateCaseNumber] ON [dbo].[tblCase] 
FOR INSERT
AS
	DECLARE @ID AS int
	
	Set @Id = (Select Id FROM Inserted)

	Declare @newCaseNumber Decimal(18,0) 

	BEGIN TRANSACTION

	SELECT @newCaseNumber  = MAX(CaseNumber) + 1 FROM tblCase

	UPDATE tblCase Set CaseNumber = @newCaseNumber WHERE Id=@ID AND CaseNumber=0

	COMMIT TRANSACTION

Go

RAISERROR ('Alter tblGDPRDataPrivacyFavorite.ProductAreas from 256 nvarchar to 1000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','ProductAreas') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblGDPRDataPrivacyFavorite]
		ALTER COLUMN ProductAreas nvarchar(1000) NULL
	End
Go

RAISERROR ('Alter tblGDPRDataPrivacyFavorite.CaseTypes from 256 nvarchar to 1000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblGDPRDataPrivacyFavorite','CaseTypes') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblGDPRDataPrivacyFavorite]
		ALTER COLUMN CaseTypes nvarchar(1000) NULL
	End
Go

RAISERROR ('Alter tblFAQ.Answer from 2000 nvarchar to 4000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('tblFAQ','Answer') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblFAQ]
		ALTER COLUMN Answer nvarchar(4000) Not NULL
	End
Go

RAISERROR ('Alter tblFAQ.Answer_Internal from 1000 nvarchar to 4000 nvarchar', 10, 1) WITH NOWAIT
IF COL_LENGTH('tblFAQ','Answer_Internal') IS NOT NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblFAQ]
		ALTER COLUMN Answer_Internal nvarchar(4000) Not NULL
	End
Go

UPDATE [dbo].[tblSettings]
SET BlockedEmailRecipients = 
    CASE 
        WHEN BlockedEmailRecipients IS NULL THEN 'noreply'
        ELSE CONCAT(BlockedEmailRecipients, ';noreply')
    END
WHERE CHARINDEX('noreply', BlockedEmailRecipients) = 0 OR BlockedEmailRecipients IS NULL;
Go


IF EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_tblFormFieldValueHistory_Case_Id'
        AND object_id = OBJECT_ID('tblFormFieldValueHistory')
)
BEGIN
    PRINT 'Index exists.'
END
ELSE
BEGIN
    CREATE INDEX IX_tblFormFieldValueHistory_Case_Id ON tblFormFieldValueHistory(Case_Id);
END
GO

RAISERROR ('Add Column ErrorMailTo to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','ErrorMailTo') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD ErrorMailTo nvarchar(200) null
	End
Go

-- #15156 - Ändra från ntext till nvarchar(max)

IF EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[tblCase]'))
	DROP FULLTEXT INDEX ON [dbo].[tblCase]
GO

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCase' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		alter table tblcase alter column [Description] nvarchar(MAX) not null
	End
Go

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCaseSolution' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		alter table tblCaseSolution alter column [Description] nvarchar(MAX) null
	End
Go

IF (SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblCaseHistory' AND COLUMN_NAME = 'Description' and DATA_TYPE = 'ntext') IS NOT NULL
	BEGIN	 
		alter table tblCaseHistory alter column [Description] nvarchar(MAX) not null
	End
Go

IF EXISTS (SELECT * from sys.fulltext_catalogs AS c where c.[name] = 'SearchCasesFTS')
BEGIN
	CREATE FULLTEXT INDEX ON dbo.tblCase
  (   
	Place Language 1033,   
	Persons_Name Language 1033,   
	Persons_EMail Language 1033,
	Caption Language 1033,	  
	Persons_Phone Language 1033,
	[Description] Language 1033,
	Miscellaneous Language 1033,
	ReportedBy Language 1033,
	InventoryNumber Language 1033,
	Available Language 1033,
	Persons_CellPhone Language 1033,
	InventoryType Language 1033,
	InventoryLocation Language 1033,
	InvoiceNumber Language 1033,
	UserCode Language 1033,
	ReferenceNumber Language 1033,
	VerifiedDescription Language 1033,
	RegUserName Language 1033,
	CostCentre Language 1033
  )  
  KEY INDEX PK_tblCase  
  ON SearchCasesFTS
          WITH STOPLIST = SYSTEM, CHANGE_TRACKING AUTO;  
END
GO

RAISERROR ('Add Column ExternalEMailSubjectPattern to tblSettings', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblSettings','ExternalEMailSubjectPattern') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblSettings]
		ADD ExternalEMailSubjectPattern nvarchar(1000) null
	End
Go

RAISERROR ('Add Column ExternalCaseNumber to tblCase', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCase','ExternalCaseNumber') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCase]
		ADD ExternalCaseNumber nvarchar(100) null
	End
Go

RAISERROR ('Add Column ExternalCaseNumber to tblCaseHistory', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCaseHistory','ExternalCaseNumber') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCaseHistory]
		ADD ExternalCaseNumber nvarchar(100) null
	End
Go


-- 5.6.0


-- Add column to tblStatus #13486
RAISERROR ('Add Column SplitOnSave to tblStatus', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblStatus','SplitOnSave') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblStatus]
		ADD SplitOnSave bit not null default 0
	End
Go

-- 5.6.1

-- Fulltextsearch on ExtendedCaseValues
-- Check if the unique index IX_ExtendedCaseValues_Id exists
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ExtendedCaseValues_Id' AND object_id = OBJECT_ID('dbo.ExtendedCaseValues'))
BEGIN
    CREATE UNIQUE INDEX IX_ExtendedCaseValues_Id ON dbo.ExtendedCaseValues(Id);
END

-- Drop existing full-text index if it exists
IF EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[ExtendedCaseValues]'))
    DROP FULLTEXT INDEX ON [dbo].[ExtendedCaseValues]
GO

-- Create full-text index on ExtendedCaseValues table
CREATE FULLTEXT INDEX ON dbo.ExtendedCaseValues  
(			 
    Value
)  
KEY INDEX IX_ExtendedCaseValues_Id
ON SearchCasesFTS  
WITH STOPLIST = SYSTEM;
GO

--License reporter
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tblCustomer' 
      AND COLUMN_NAME = 'ERPContractNumber'
)
BEGIN
    ALTER TABLE tblCustomer
    ADD ERPContractNumber NVARCHAR(12) NULL;
END;
GO

-- 5.7.0 

RAISERROR ('Add Column MovedFromCustomer_Id to tblCase', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCase','MovedFromCustomer_Id') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCase]
		ADD MovedFromCustomer_Id int null
	End
Go

WITH LastDifferentCustomer AS (
    SELECT
        h.Case_Id,
        h.Customer_Id AS MovedFromCustomer_Id,
        ROW_NUMBER() OVER (PARTITION BY h.Case_Id ORDER BY h.CreatedDate DESC) AS rn
    FROM
        tblCaseHistory h
    INNER JOIN tblCase c
        ON h.Case_Id = c.Id
    WHERE
        h.Customer_Id != c.Customer_Id
)

UPDATE c
SET
    c.MovedFromCustomer_Id = ldc.MovedFromCustomer_Id
FROM
    tblCase c
INNER JOIN LastDifferentCustomer ldc
    ON c.Id = ldc.Case_Id
WHERE
    ldc.rn = 1
	and c.MovedFromCustomer_Id is null;

Go

RAISERROR ('Checking and creating sequence [dbo].[CaseNumberSequence] if it does not exist', 10, 1) WITH NOWAIT
-- Declare a variable to hold the calculated start value
DECLARE @StartValue INT;

-- Calculate the start value based on the max(caseNumber) + 100
SELECT @StartValue = ISNULL(MAX(caseNumber), 0) + 100 FROM dbo.tblCase WITH (TABLOCKX);

-- Check if the sequence already exists
IF NOT EXISTS (SELECT * FROM sys.sequences WHERE name = 'CaseNumberSequence' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
	-- If the sequence does not exist, create it
	RAISERROR ('Creating sequence [dbo].[CaseNumberSequence]', 10, 1) WITH NOWAIT
	DECLARE @SQL NVARCHAR(MAX);
	SET @SQL = '
		CREATE SEQUENCE [dbo].[CaseNumberSequence]
		START WITH ' + CAST(@StartValue AS NVARCHAR(20)) + '
		INCREMENT BY 1
		MINVALUE 200
		NO CACHE';  -- Disable cache to avoid any issues with cached values during system restarts

	EXEC(@SQL);

	RAISERROR ('Sequence [dbo].[CaseNumberSequence] created successfully', 10, 1) WITH NOWAIT
END
ELSE
BEGIN
	-- If the sequence already exists, output a message
	RAISERROR ('Sequence [dbo].[CaseNumberSequence] already exists', 10, 1) WITH NOWAIT
END
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_CreateCaseNumber' AND parent_id = OBJECT_ID('dbo.tblCase'))
BEGIN
    -- If the trigger exists, alter it using dynamic SQL
	PRINT 'Altering Trigger'
    EXEC('
    ALTER TRIGGER [dbo].[TR_CreateCaseNumber] 
    ON [dbo].[tblCase]
    AFTER INSERT
    AS
    BEGIN
        -- Obtain an exclusive lock to prevent concurrent access to the sequence
        EXEC sp_getapplock @Resource = ''CaseNumberSequenceLock'', @LockMode = ''Exclusive'';

        -- Set the CaseNumber for the newly inserted rows
        UPDATE e
        SET e.CaseNumber = NEXT VALUE FOR CaseNumberSequence
        FROM dbo.tblCase e
        INNER JOIN inserted i ON e.Id = i.Id;

        -- Release the lock after updating
        EXEC sp_releaseapplock @Resource = ''CaseNumberSequenceLock'';
    END;')
END
ELSE
BEGIN
    -- If the trigger does not exist, create it using dynamic SQL
	PRINT 'Creating Trigger'
    EXEC('
    CREATE TRIGGER [dbo].[TR_CreateCaseNumber] 
    ON [dbo].[tblCase]
    AFTER INSERT
    AS
    BEGIN
        -- Obtain an exclusive lock to prevent concurrent access to the sequence
        EXEC sp_getapplock @Resource = ''CaseNumberSequenceLock'', @LockMode = ''Exclusive'';

        -- Set the CaseNumber for the newly inserted rows
        UPDATE e
        SET e.CaseNumber = NEXT VALUE FOR CaseNumberSequence
        FROM dbo.tblCase e
        INNER JOIN inserted i ON e.Id = i.Id;

        -- Release the lock after updating
        EXEC sp_releaseapplock @Resource = ''CaseNumberSequenceLock'';
    END;')
END
GO

ALTER PROCEDURE [dbo].[sp_DeleteCases] 
	@Cases IdsList READONLY
AS
BEGIN
	BEGIN TRY

		IF (OBJECT_ID ('tblLink_tblUsers', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLink', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE lu
				FROM tblLink_tblUsers AS lu
				INNER JOIN dbo.tblLink AS l
					ON lu.Link_Id = l.Id
				INNER JOIN dbo.tblCaseSolution  AS cs 
					ON l.CaseSolution_Id= cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLink_tblWorkingGroup', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLink', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE lw
				FROM tblLink_tblWorkingGroup AS lw
				INNER JOIN tblLink AS l	
					ON lw.Link_Id = l.Id
				INNER JOIN dbo.tblCaseSolution AS cs
					ON l.CaseSolution_Id = cs.Id
				INNER JOIN tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChangeLog', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChangeEMailLog', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblChangeHistory', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cl
				FROM tblChangeLog AS cl
				INNER JOIN dbo.tblChangeEMailLog AS cel
					ON cl.ChangeEMailLog_Id = cel.Id
				INNER JOIN dbo.tblChangeHistory AS chh
					ON cel.ChangeHistory_Id = chh.Id
				INNER JOIN dbo.tblChange AS ch
					on chh.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					on ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblParentChildCaseRelations', 'U') IS NOT NULL)
			BEGIN
				DELETE tpr 
				FROM tblParentChildCaseRelations AS tpr
				INNER JOIN @Cases AS c
					ON c.Id = tpr.Descendant_Id;
			END

		IF (OBJECT_ID ('tblParentChildCaseRelations', 'U') IS NOT NULL)
			BEGIN
				DELETE tpr 
				FROM tblParentChildCaseRelations AS tpr
				INNER JOIN @Cases AS c
					ON c.Id = tpr.Ancestor_Id;
			END

		IF (OBJECT_ID ('tblCase_ExtendedCaseData', 'U') IS NOT NULL)
			BEGIN
				DELETE tec
				FROM tblCase_ExtendedCaseData AS tec
				INNER JOIN @Cases AS c
					ON c.Id = tec.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseIsAbout', 'U') IS NOT NULL)
			BEGIN
				DELETE tca
				FROM tblCaseIsAbout AS tca
				INNER JOIN @Cases AS c
					ON c.Id = tca.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseQuestion', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblCaseQuestionCategory', 'U') IS NOT NULL  AND
			OBJECT_ID ('tblCaseQuestionHeader', 'U') IS NOT NULL)
			BEGIN
				DELETE cq
				FROM tblCaseQuestion AS cq
				INNER JOIN dbo.tblCaseQuestionCategory  AS cqc
					on cq.CaseQuestionCategory_Id = cqc.Id
				INNER JOIN dbo.tblCaseQuestionHeader AS cqh
					on cqc.CaseQuestionHeader_Id = cqh.Id
				INNER JOIN @Cases AS c
					on cqh.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseQuestionCategory', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblCaseQuestionHeader', 'U') IS NOT NULL)
			BEGIN
				DELETE cqc
				FROM tblCaseQuestionCategory AS cqc
				INNER JOIN dbo.tblCaseQuestionHeader AS cqh
					on cqc.CaseQuestionHeader_Id = cqh.Id
				INNER JOIN @Cases AS c
					on cqh.Case_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblCaseQuestionHeader', 'U') IS NOT NULL)
			BEGIN
 				DELETE cqh
				FROM tblCaseQuestionHeader AS cqh
				INNER JOIN @Cases AS c 
					ON cqh.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLogFileExisting', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN
 				DELETE lfe
				FROM tblLogFileExisting AS lfe
				INNER JOIN dbo.tblLog AS l
					ON lfe.Log_Id = l.Id
				INNER JOIN @Cases AS c 
					ON l.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLogFileExisting', 'U') IS NOT NULL)
			BEGIN
				DELETE lfe
				FROM tblLogFileExisting AS lfe
				INNER JOIN @Cases AS c 
					ON lfe.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLogFileExisting', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN
				DELETE cd
				FROM tblChange_tblDepartment AS cd
				INNER JOIN dbo.tblChange AS ch
					ON cd.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChange_tblChangeGroup', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE ccg
				FROM tblChange_tblChangeGroup AS ccg
				INNER JOIN tblChange AS ch
					ON ccg.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblCaseInvoiceRow', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN
	 			DELETE cir
				FROM tblCaseInvoiceRow AS cir
				INNER JOIN tblInvoiceRow AS ir 
					ON cir.InvoiceRow_Id = ir.Id
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseInvoiceRow', 'U') IS NOT NULL)
			BEGIN
				DELETE cir
				FROM tblCaseInvoiceRow AS cir
				INNER JOIN @Cases AS c
					ON cir.Case_Id = c.Id;			
			END

		IF (OBJECT_ID ('tblChangeCouncil', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cc
				FROM tblChangeCouncil AS cc
				INNER JOIN tblChange AS ch
					ON cc.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChangeContact', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cc
				FROM tblChangeContact AS cc
				INNER JOIN dbo.tblChange AS ch
					ON cc.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblChange_tblChange', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cch
				FROM tblChange_tblChange AS cch
				INNER JOIN dbo.tblChange AS ch
					ON cch.RelatedChange_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;	
			END

		IF (OBJECT_ID ('tblChangeFile', 'U') IS NOT NULL  AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cf
				FROM tblChangeFile AS cf
				INNER JOIN dbo.tblChange AS ch
					ON cf.Change_Id = ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN
				SELECT tl.Id, tl.Case_Id, tl.InvoiceRow_Id 
				INTO #TmpLogs
				FROM tblLog AS tl 
				INNER JOIN @Cases AS c
					ON c.Id = tl.Case_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLogFile', 'U') IS NOT NULL)
			BEGIN
				DELETE lf
				FROM tblLogFile AS lf
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = lf.Log_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblMail2Ticket', 'U') IS NOT NULL)
			BEGIN
				DELETE m2t
				FROM tblMail2Ticket AS m2t
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = m2t.Log_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblMail2TicketCase', 'U') IS NOT NULL)
			BEGIN
				DELETE m2tc
				FROM tblMail2TicketCase AS m2tc
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = m2tc.Case_Id;
			END

		IF (OBJECT_ID ('tblMail2Ticket', 'U') IS NOT NULL)
			BEGIN
				DELETE m2t
				FROM tblMail2Ticket AS m2t
				INNER JOIN @Cases AS c
					ON c.Id = m2t.Case_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLogAttempts', 'U') IS NOT NULL)
			BEGIN
				DELETE ela
				FROM tblEmailLogAttempts AS ela
				INNER JOIN #TmpLogs AS tmp
					ON tmp.Id = ela.EmailLog_Id;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL)
			BEGIN
				SELECT ch.Id, ch.Case_Id
				INTO #TmpCaseHistory
				FROM tblCaseHistory AS ch
				INNER JOIN @Cases AS c
					ON ch.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLogAttempts', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLog', 'U') IS NOT NULL)
			BEGIN
				DELETE ela
				FROM tblEmailLogAttempts AS ela
				INNER JOIN tblEmailLog AS el
					ON ela.EmailLog_Id = el.Id
				INNER JOIN #TmpCaseHistory AS ch
					ON ch.Id = el.CaseHistory_Id;
			END

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEmailLog', 'U') IS NOT NULL)
			BEGIN			
				DELETE el
				FROM tblEmailLog AS el
				INNER JOIN #TmpCaseHistory AS ch
					ON ch.Id = el.CaseHistory_Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN	
				SELECT ir.Id, ir.Case_Id, ir.InvoiceHeader_Id
				INTO #tmpInvoiceRow
				FROM tblInvoiceRow AS ir
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblEMailLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblInvoiceHeader', 'U') IS NOT NULL)
			BEGIN	
				DELETE el 
				FROM tblEMailLog AS el
				INNER JOIN tblLog AS tl
					ON el.Log_Id = tl.Id
				INNER JOIN tblInvoiceRow AS ir
					ON tl.InvoiceRow_Id = ir.Id
				INNER JOIN tblInvoiceHeader AS ih
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN #tmpInvoiceRow AS tir
					ON tir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON tir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblEMailLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE el
				FROM tblEmailLog AS el
				INNER JOIN #TmpLogs As l
					ON l.Id = el.Log_Id;
			END
			
		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN tblInvoiceRow AS ir
					ON ir.Id = tl.InvoiceRow_Id
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblInvoiceHeader', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN tblInvoiceRow AS ir
					ON tl.InvoiceRow_Id = ir.Id
				INNER JOIN tblInvoiceHeader AS ih
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN #tmpInvoiceRow AS tir
					ON tir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON tir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN #tmpInvoiceRow as r
					ON r.Id = tl.InvoiceRow_Id;
			END

		IF (OBJECT_ID ('tblLog', 'U') IS NOT NULL)
			BEGIN	
				DELETE tl
				FROM tblLog AS tl
				INNER JOIN #TmpLogs As l
					ON l.Id = tl.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN	
				DELETE ir
				FROM tblInvoiceRow AS ir
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL)
			BEGIN	
				DELETE ir
				FROM tblInvoiceRow AS ir
				INNER JOIN tblInvoiceHeader AS ih
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN #tmpInvoiceRow AS tir
					ON tir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON tir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpLogs') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpLogs;
			END

		IF (OBJECT_ID ('tblInvoiceRow', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblInvoiceHeader', 'U') IS NOT NULL)
			BEGIN	
				DELETE ih
				FROM tblInvoiceHeader AS ih
				INNER JOIN #tmpInvoiceRow AS ir
					ON ir.InvoiceHeader_Id = ih.Id
				INNER JOIN @Cases AS c
					ON ir.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tempdb..#tmpInvoiceRow') IS NOT NULL)
			BEGIN			
				DROP TABLE #tmpInvoiceRow;
			END

		RAISERROR('Deleting from dbo.tblCaseHistory:', 10, 1) WITH NOWAIT;

		IF (OBJECT_ID ('tblCaseHistory', 'U') IS NOT NULL)
			BEGIN	
				--delete from dbo.tblCaseHistory
				--from dbo.tblCaseHistory
				--inner join dbo.tblEMailLog on dbo.tblCaseHistory.Id=dbo.tblEMailLog.CaseHistory_Id
				--inner join dbo.tblLog on dbo.tblEMailLog.Log_Id=dbo.tblLog.Id
				--inner join dbo.tblCase on dbo.tblLog.Case_Id=dbo.tblCase.Id
				--where tblCase.Id IN (SELECT Id from @Cases)


				DELETE ch
				FROM tblCaseHistory AS ch
				INNER JOIN #TmpCaseHistory AS t
					ON t.Id = ch.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpCaseHistory') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpCaseHistory;
			END

		RAISERROR('Deleting from dbo.tblCaseLock:', 10, 1) WITH NOWAIT;

		IF (OBJECT_ID ('tblCaseLock', 'U') IS NOT NULL)
			BEGIN	
				DELETE cl
				FROM tblCaseLock AS cl
				INNER JOIN  @Cases AS c
					ON c.Id = cl.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseFile', 'U') IS NOT NULL)
			BEGIN
				DELETE cf
				FROM tblCaseFile AS cf
				INNER JOIN  @Cases AS c
					ON c.Id = cf.Case_Id;
			END

		IF (OBJECT_ID ('tblFileViewLog', 'U') IS NOT NULL)
			BEGIN
				DELETE fvl
				FROM tblFileViewLog AS fvl
				INNER JOIN  @Cases AS c
					ON c.Id = fvl.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				SELECT ci.Id
				INTO #TmpCaseInvoice
				FROM tblCaseInvoice AS ci
				INNER JOIN  @Cases AS c
					ON c.Id = ci.CaseId;
			END

		IF (OBJECT_ID ('tblCaseInvoiceArticle', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoiceOrder', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE cia
				FROM tblCaseInvoiceArticle AS cia
				INNER JOIN tblCaseInvoiceOrder AS cio
					ON cio.Id = cia.OrderId
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = cio.InvoiceId;
			END

		IF (OBJECT_ID ('tblCaseInvoiceOrderFile', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoiceOrder', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE cof
				FROM tblCaseInvoiceOrderFile AS cof
				INNER JOIN tblCaseInvoiceOrder AS cio
					ON cio.Id = cof.OrderId
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = cio.InvoiceId;
			END

		IF (OBJECT_ID ('tblCaseInvoiceOrder', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE cio
				FROM tblCaseInvoiceOrder AS cio
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = cio.InvoiceId;
			END

		IF (OBJECT_ID ('tblCaseInvoice', 'U') IS NOT NULL)
			BEGIN
				DELETE ci
				FROM tblCaseInvoice AS ci
				INNER JOIN #TmpCaseInvoice AS t
					ON t.Id = ci.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpCaseInvoice') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpCaseInvoice;
			END

		IF (OBJECT_ID ('tblCaseSolutionSchedule', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE csc
				FROM tblCaseSolutionSchedule AS csc
				INNER JOIN tblCaseSolution AS cs
					ON csc.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolutionFieldSettings', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE csf 
				FROM tblCaseSolutionFieldSettings  csf
				INNER JOIN tblCaseSolution AS cs
					ON csf.CaseSolution_Id = cs.Id
				INNER JOIN tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblLink', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
 				DELETE l
				FROM dbo.tblLink AS l
				INNER JOIN tblCaseSolution AS cs
					ON l.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolution_tblCaseSection_ExtendedCaseForm', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cse
				FROM tblCaseSolution_tblCaseSection_ExtendedCaseForm AS cse
				INNER JOIN dbo.tblCaseSolution AS cs
					on cse.tblCaseSolutionID = cs.Id
				INNER JOIN dbo.tblChange AS ch
					on cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					on ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolution_ExtendedCaseForms', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cse
				FROM tblCaseSolution_ExtendedCaseForms AS cse
				INNER JOIN dbo.tblCaseSolution AS cs 
					ON cse.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblCaseSolutionCondition', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE csc
				FROM tblCaseSolutionCondition AS csc
				INNER JOIN tblCaseSolution AS cs 
					ON csc.CaseSolution_Id=cs.Id
				INNER JOIN tblChange AS ch
					ON cs.Change_Id=ch.Id
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id; 
			END

		IF (OBJECT_ID ('tblCaseSolutionConditionLog', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblCaseSolution', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE cscl
				FROM tblCaseSolutionConditionLog AS cscl
				INNER JOIN dbo.tblCaseSolution AS cs
					ON cscl.CaseSolution_Id = cs.Id
				INNER JOIN dbo.tblChange AS ch
					ON cs.Change_Id = ch.Id
				INNER JOIN @Cases AS c 
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblChange', 'U') IS NOT NULL)
			BEGIN
				DELETE ch
				FROM tblChange AS ch
				INNER JOIN @Cases AS c
					ON ch.SourceCase_Id = c.Id;
			END

		IF (OBJECT_ID ('tblContractLog', 'U') IS NOT NULL)
			BEGIN
				DELETE cl
				FROM tblContractLog AS cl
				INNER JOIN  @Cases AS c
					ON c.Id = cl.Case_Id;
			END	

		IF (OBJECT_ID ('tblCaseFollowUps', 'U') IS NOT NULL)
			BEGIN
				DELETE flu
				FROM tblCaseFollowUps AS flu
				INNER JOIN  @Cases AS c
					ON c.Id = flu.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseExtraFollowers', 'U') IS NOT NULL)
			BEGIN
				DELETE ef
				FROM tblCaseExtraFollowers AS ef
				INNER JOIN  @Cases AS c
					ON c.Id = ef.Case_Id;
			END

		IF (OBJECT_ID ('tblCase_tblCaseSection_ExtendedCaseData', 'U') IS NOT NULL)
			BEGIN
				DELETE tecd
				FROM tblCase_tblCaseSection_ExtendedCaseData AS tecd
				INNER JOIN  @Cases AS c
					ON c.Id = tecd.Case_Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL)
			BEGIN
				SELECT cp.Id
				INTO #TmpQuestionnaireCircularPart
				FROM tblQuestionnaireCircularPart AS cp
				INNER JOIN  @Cases AS c
					ON c.Id = cp.Case_Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblQuestionnaireResult', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblQuestionnaireQuestionResult', 'U') IS NOT NULL)
			BEGIN
				DELETE qqr
				FROM tblQuestionnaireResult AS qr			
				INNER JOIN #TmpQuestionnaireCircularPart AS cp
					ON qr.QuestionnaireCircularPartic_Id = cp.Id
				INNER JOIN tblQuestionnaireQuestionResult AS qqr
					ON qqr.QuestionnaireResult_Id = qr.Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL AND 
			OBJECT_ID ('tblQuestionnaireResult', 'U') IS NOT NULL)
			BEGIN
				DELETE qr
				FROM tblQuestionnaireResult AS qr			
				INNER JOIN #TmpQuestionnaireCircularPart AS cp
					ON qr.QuestionnaireCircularPartic_Id = cp.Id;
			END

		IF (OBJECT_ID ('tblQuestionnaireCircularPart', 'U') IS NOT NULL)
			BEGIN
				DELETE cp
				FROM tblQuestionnaireCircularPart AS cp
				INNER JOIN #TmpQuestionnaireCircularPart AS t
					ON t.Id = cp.Id;
			END

		IF (OBJECT_ID ('tempdb..#TmpQuestionnaireCircularPart') IS NOT NULL)
			BEGIN	
				DROP TABLE #TmpQuestionnaireCircularPart;
			END

		IF (OBJECT_ID ('tblMergedCases', 'U') IS NOT NULL)
			BEGIN
				DELETE mc
				FROM tblMergedCases AS mc
				INNER JOIN  @Cases AS c
					ON c.Id = mc.MergedChild_Id OR c.Id = mc.MergedParent_Id;
			END

		IF (OBJECT_ID ('tblLogProgram', 'U') IS NOT NULL)
			BEGIN
				DELETE lp 
				FROM tblLogProgram AS lp
				INNER JOIN @Cases AS c
					ON c.Id = lp.Case_Id;
			END

		IF (OBJECT_ID ('tblLocalAdmin', 'U') IS NOT NULL)
			BEGIN
				DELETE la 
				FROM tblLocalAdmin AS la
				INNER JOIN @Cases AS c
					ON c.Id = la.Case_Id;		
			END

		IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL)
			BEGIN
				DELETE vh
				FROM tblFormFieldValueHistory AS vh
				INNER JOIN @Cases AS c
					ON c.Id = vh.Case_Id;	
			END

		IF (OBJECT_ID ('tblFormFieldValueHistory', 'U') IS NOT NULL AND
			OBJECT_ID ('tblFormFieldValue', 'U') IS NOT NULL)
			BEGIN
				DELETE vh
				FROM tblFormFieldValueHistory AS vh
				INNER JOIN tblFormFieldValue AS fv 
					ON fv.Case_Id = vh.FormField_Id
				INNER JOIN @Cases AS c 
					ON fv.Case_Id = c.Id;
			END

		IF (OBJECT_ID ('tblFormFieldValue', 'U') IS NOT NULL)
			BEGIN
				DELETE tfv
				FROM tblFormFieldValue AS tfv
				INNER JOIN @Cases AS c
					ON c.Id = tfv.Case_Id;
			END

		IF (OBJECT_ID ('tblCaseStatistics', 'U') IS NOT NULL)
			BEGIN
				DELETE cs
				FROM tblCaseStatistics AS cs
				INNER JOIN @Cases AS c
					ON c.Id = cs.Case_Id;			 
			END

		IF (OBJECT_ID ('tblChecklistRow', 'U') IS NOT NULL)
			BEGIN
				DELETE cr
				FROM tblChecklistRow as cr
				INNER JOIN @Cases AS c
					ON c.Id = cr.Case_Id;
			END

		IF (OBJECT_ID ('tblCase', 'U') IS NOT NULL)
			BEGIN
				DELETE tc
				FROM tblCase AS tc
				INNER JOIN  @Cases AS c
					ON tc.Id = c.Id;
			END
	END TRY

	BEGIN CATCH

		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
		DECLARE @ErrorState INT = ERROR_STATE()

		-- Use RAISERROR inside the CATCH block to return error  
		-- information about the original error that caused  
		-- execution to jump to the CATCH block.  
		RAISERROR (@ErrorMessage, -- Message text.  
				   @ErrorSeverity, -- Severity.  
				   @ErrorState -- State.  
				   );
	END CATCH
END	
GO

-- 5.7.1 
-- Changes for 5.7.1 goes here

-- 5.7.2 
-- Changes for 5.7.2 goes here

-- 5.7.3
-- Changes for 5.7.3 goes here

RAISERROR ('Add Column OriginWorkingGroup_Id to tblCase', 10, 1) WITH NOWAIT
IF COL_LENGTH('dbo.tblCase','OriginWorkingGroup_Id') IS NULL
	BEGIN	 
		ALTER TABLE [dbo].[tblCase]
		ADD OriginWorkingGroup_Id int null
	End
Go

-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.7.3'
GO

