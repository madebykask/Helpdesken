
ALTER TABLE [dbo].[tblComputerUsers]
    ADD [ComputerUsersCategoryID] INT NULL;

GO
CREATE NONCLUSTERED INDEX [IX_tblComputerUsers_ComputerUSersCategory]
    ON [dbo].[tblComputerUsers]([Customer_Id] ASC, [ComputerUsersCategoryID] ASC, [Status] ASC)
    INCLUDE([Id], [UserId], [FirstName], [SurName], [Location], [Phone], [Cellphone], [Email], [UserCode], [Department_Id], [OU_Id], [CostCentre]);



GO
CREATE TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] (
    [Case_Id]             INT NOT NULL,
    [CaseSection_Id]      INT NOT NULL,
    [ExtendedCaseData_Id] INT NOT NULL,
    CONSTRAINT [PK_tblCase_tblCaseSection_ExtendedCaseData] PRIMARY KEY CLUSTERED ([Case_Id] ASC, [CaseSection_Id] ASC, [ExtendedCaseData_Id] ASC)
);


GO
CREATE TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] (
    [ID]                 INT IDENTITY (1, 1) NOT NULL,
    [tblCaseSolutionID]  INT NOT NULL,
    [tblCaseSectionID]   INT NOT NULL,
    [ExtendedCaseFormID] INT NOT NULL,
    CONSTRAINT [PK_tblCaseSolution_tblCaseSection_ExtendedCaseForm] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO
CREATE TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] (
    [ComputerUserCategoryID] INT NOT NULL,
    [ExtendedCaseFormID]     INT NOT NULL,
    CONSTRAINT [PK_tblComputerUserCategory_ExtendedCaseForm_1] PRIMARY KEY CLUSTERED ([ComputerUserCategoryID] ASC)
);

GO
CREATE TABLE [dbo].[tblComputerUsersCategory] (
    [ID]                        INT              IDENTITY (1, 1) NOT NULL,
    [Name]                      NVARCHAR (MAX)   NOT NULL,
    [CaseSolutionID]            INT              NULL,
    [ComputerUsersCategoryGuid] UNIQUEIDENTIFIER NOT NULL,
    [IsReadOnly]                BIT              NOT NULL,
    [CustomerID]                INT              NOT NULL,
    [ExtendedCaseFormID]        INT              NULL,
    CONSTRAINT [PK_tblComputerUsersCategory] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO
ALTER TABLE [dbo].[tblComputerUsersCategory]
    ADD CONSTRAINT [DF_tblComputerUsersCategory_ReadOnly] DEFAULT ((0)) FOR [IsReadOnly];

GO
ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_tblCase] FOREIGN KEY ([Case_Id]) REFERENCES [dbo].[tblCase] ([Id]);

GO
ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_tblCaseSections] FOREIGN KEY ([CaseSection_Id]) REFERENCES [dbo].[tblCaseSections] ([Id]);

GO
ALTER TABLE [dbo].[tblCase_tblCaseSection_ExtendedCaseData] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCase_tblCaseSection_ExtendedCaseData_ExtendedCaseData] FOREIGN KEY ([ExtendedCaseData_Id]) REFERENCES [dbo].[ExtendedCaseData] ([Id]);

GO
ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSolution] FOREIGN KEY ([tblCaseSolutionID]) REFERENCES [dbo].[tblCaseSolution] ([Id]);

GO
ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_tblCaseSections] FOREIGN KEY ([tblCaseSectionID]) REFERENCES [dbo].[tblCaseSections] ([Id]);

GO
ALTER TABLE [dbo].[tblCaseSolution_tblCaseSection_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblCaseSolution_tblCaseSection_ExtendedCaseForm_ExtendedCaseForms] FOREIGN KEY ([ExtendedCaseFormID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);

GO
ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_tblComputerUsersCategory] FOREIGN KEY ([ComputerUserCategoryID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);


ALTER TABLE [dbo].[tblComputerUserCategory_ExtendedCaseForm] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUserCategory_ExtendedCaseForm_ExtendedCaseForms] FOREIGN KEY ([ExtendedCaseFormID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);


ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_tblComputerUsersCategory] FOREIGN KEY ([ID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);


GO
ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_ExtendedCaseForms1] FOREIGN KEY ([ExtendedCaseFormID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);

GO
ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_tblCustomer] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tblCustomer] ([Id]);


GO
ALTER TABLE [dbo].[tblComputerUsersCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsersCategory_ExtendedCaseForms] FOREIGN KEY ([ID]) REFERENCES [dbo].[ExtendedCaseForms] ([Id]);


GO
ALTER TABLE [dbo].[tblComputerUsers] WITH NOCHECK
    ADD CONSTRAINT [FK_tblComputerUsers_tblComputerUsersCategory] FOREIGN KEY ([ComputerUsersCategoryID]) REFERENCES [dbo].[tblComputerUsersCategory] ([ID]);
