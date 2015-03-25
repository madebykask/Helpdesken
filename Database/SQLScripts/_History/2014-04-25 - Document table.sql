

ALTER TABLE [dbo].[tblDocument] 
DROP CONSTRAINT [DF_tblDocument_ShowOnStartPage]  

ALTER TABLE dbo.tblDocument
DROP COLUMN [ShowOnStartPage]

ALTER TABLE dbo.tblDocument
ADD [ShowOnStartPage] BIT NOT NULL
CONSTRAINT [DF_tblDocument_ShowOnStartPage]  
DEFAULT ((1))
