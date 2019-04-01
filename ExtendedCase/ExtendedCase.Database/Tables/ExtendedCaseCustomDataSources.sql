CREATE TABLE [dbo].[ExtendedCaseCustomDataSources]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DataSourceId] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(500) NULL, 
    [MetaData] NVARCHAR(MAX) NOT NULL,
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [UpdatedBy] NVARCHAR(50) NULL
)
