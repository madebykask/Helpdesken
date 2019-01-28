CREATE TABLE [dbo].[ExtendedCaseAssignments]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ExtendedCaseFormId] INT NOT NULL, 
    [UserRole] INT NULL, 
    [CaseStatus] INT NULL, 
    [CustomerId] INT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [UpdatedBy] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_EFormAssignments_ExtendedCaseForms] FOREIGN KEY ([ExtendedCaseFormId]) REFERENCES [ExtendedCaseForms]([Id])
)
