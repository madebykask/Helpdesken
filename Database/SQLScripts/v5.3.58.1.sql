--update DB from 5.3.57 to 5.3.58.1 version

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
-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.58.1'
GO