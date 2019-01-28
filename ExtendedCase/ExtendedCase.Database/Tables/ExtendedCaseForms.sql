CREATE TABLE [dbo].[ExtendedCaseForms]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MetaData] NVARCHAR(MAX) NOT NULL, 
    [Description] NVARCHAR(500) NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [UpdatedBy] NVARCHAR(50) NULL, 
    [Name] NVARCHAR(100) NULL, 
    [Guid] UNIQUEIDENTIFIER NULL, 
    [Status] INT NOT NULL, 
    [Version] INT NOT NULL
)
