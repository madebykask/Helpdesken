--update DB from 5.3.42 to 5.3.43 version

/*-- M2T EmailAnswer Separator scripts
----------------------------------------------------------------------------------------------*/

if exists (select * from syscolumns inner join sysobjects on sysobjects.id = syscolumns.id              
		 where syscolumns.name = N'EMailAnswerSeparator' and sysobjects.name = N'tblSettings')
BEGIN
    ALTER TABLE tblSettings
    ALTER COLUMN EMailAnswerSeparator nvarchar(512) not null    
END
GO

RAISERROR ('Add email answer separator regular expressions for all customers', 10, 1) WITH NOWAIT
GO

BEGIN TRANSACTION 
GO 

DECLARE @count int
DECLARE @curRow int
DECLARE @Customers as Table(RowNumber int, CustomerId int)

--select all customers that have settings
INSERT INTO @Customers (RowNumber, CustomerId)
SELECT (ROW_NUMBER() OVER (Order by cus.Id)) as RowNumber, cus.Id from tblCustomer cus 
    INNER JOIN tblSettings st ON cus.Id = st.Customer_Id
ORDER BY cus.Id

select @count = COUNT(*) from @Customers
SET @curRow = 1

while(@curRow <= @count)
BEGIN
    DECLARE @customerId int = 0	     
    DECLARE @emailSep nvarchar(512)
    DECLARE @newEmailSep nvarchar(512)
    DECLARE @regex1 nvarchar(128)
    DECLARE @regex2 nvarchar(128)

    -- finds reply quote text in mail of type: <date> <time> <optional word> <email> <optional word>. 
    SET @regex1 = '^.* [0-9]{1,2}:[0-9]{1,2} (\w{0,}\s{0,})?<?(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}))>?(\s{0,}\w{0,})?:\r?$'
    
    -- finds reply quote text: <email> <optional text> <date> <time>:
    SET @regex2 = '^.* <?(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}))>? (.*)\s?[0-9]{1,2}:[0-9]{1,2}:\r?$'

    select @customerId = CustomerId from @Customers where RowNumber = @curRow    
    
    --get customer current email answer separator
    select @emailSep = ISNULL(EMailAnswerSeparator, '') from tblSettings where Customer_Id = @customerId
    SET @newEmailSep = @emailSep

    -- add first regex pattern
    IF (@emailSep = '' OR CHARINDEX(@regex1, @emailSep) = 0)
    BEGIN		  
	   IF (len(@newEmailSep) > 0) SET @newEmailSep = @newEmailSep + ';'		  
	   SET @newEmailSep = @newEmailSep + @regex1		  		
    END
	   
    -- add second regex pattern
    IF (@emailSep = '' OR CHARINDEX(@regex2, @emailSep) = 0)
    BEGIN
	   IF (len(@newEmailSep) > 0) SET @newEmailSep = @newEmailSep + ';'		  
	   SET @newEmailSep = @newEmailSep + @regex2		  
    END    

    IF (len(@newEmailSep) <> len(@emailSep))
    BEGIN 
	   -- updating email answer separator
	   UPDATE tblSettings
	   SET EMailAnswerSeparator = @newEmailSep
	   WHERE Customer_Id = @customerId
		  
	   --PRINT 'Updated customer: ' + cast(@customerId as nvarchar(4))
	   --PRINT 'EmailAnswerSeparator:' + @newEmailSep
	   --PRINT '----------------------------------------------------'
    END	       
    SET @curRow += 1
End
GO 

select Customer_Id, EMailAnswerSeparator from tblSettings 
GO

COMMIT
GO
/*----------------------------------------------------------------------------------------------*/


-- Last Line to update database version
UPDATE tblGlobalSettings SET HelpdeskDBVersion = '5.3.43'
GO

--ROLLBACK --TMP
