DECLARE @CustomerId INT
SET @CustomerId = 23


DELETE FROM [dbo].[tblCaseInvoiceArticle]
DELETE FROM [dbo].[tblInvoiceArticle]
DELETE FROM [dbo].[tblInvoiceArticleUnit]

DECLARE @HumanResources INT
DECLARE @IT INT
DECLARE @Recruitment INT
DECLARE @LearningTraining INT
DECLARE @Celebration INT
DECLARE @NewEmployee INT

SET @HumanResources = (SELECT [Id] FROM [dbo].[tblProductArea] WHERE [ProductArea] = 'Human Resources' AND [Customer_Id] = @CustomerId)
IF @HumanResources IS NULL
BEGIN
	INSERT INTO [dbo].[tblProductArea] ([ProductArea], [Customer_Id]) VALUES ('Human Resources', @CustomerId)
	SET @HumanResources = SCOPE_IDENTITY(); 
END

SET @IT = (SELECT [Id] FROM [dbo].[tblProductArea] WHERE [ProductArea] = 'IT' AND [Customer_Id] = @CustomerId)
IF @IT IS NULL
BEGIN
	INSERT INTO [dbo].[tblProductArea] ([ProductArea], [Customer_Id]) VALUES ('IT', @CustomerId)
	SET @IT = SCOPE_IDENTITY(); 
END

SET @Recruitment = (SELECT [Id] FROM [dbo].[tblProductArea] WHERE [ProductArea] = 'Recruitment' AND [Customer_Id] = @CustomerId)
IF @Recruitment IS NULL
BEGIN
	INSERT INTO [dbo].[tblProductArea] ([ProductArea], [Customer_Id], [Parent_ProductArea_Id]) VALUES ('Recruitment', @CustomerId, @HumanResources)
	SET @Recruitment = SCOPE_IDENTITY(); 
END

SET @LearningTraining = (SELECT [Id] FROM [dbo].[tblProductArea] WHERE [ProductArea] = 'Learning & Traning' AND [Customer_Id] = @CustomerId)
IF @LearningTraining IS NULL
BEGIN
	INSERT INTO [dbo].[tblProductArea] ([ProductArea], [Customer_Id], [Parent_ProductArea_Id]) VALUES ('Learning & Traning', @CustomerId, @HumanResources)
	SET @LearningTraining = SCOPE_IDENTITY(); 
END

SET @Celebration = (SELECT [Id] FROM [dbo].[tblProductArea] WHERE [ProductArea] = 'Celebration' AND [Customer_Id] = @CustomerId)
IF @Celebration IS NULL
BEGIN 
	INSERT INTO [dbo].[tblProductArea] ([ProductArea], [Customer_Id], [Parent_ProductArea_Id]) VALUES ('Celebration', @CustomerId, @HumanResources)
	SET @Celebration = SCOPE_IDENTITY(); 
END

SET @NewEmployee = (SELECT [Id] FROM [dbo].[tblProductArea] WHERE [ProductArea] = 'New employee' AND [Customer_Id] = @CustomerId)
IF @NewEmployee IS NULL
BEGIN 
	INSERT INTO [dbo].[tblProductArea] ([ProductArea], [Customer_Id], [Parent_ProductArea_Id]) VALUES ('New employee', @CustomerId, @IT)
	SET @NewEmployee = SCOPE_IDENTITY(); 
END


DECLARE @Styck INT
INSERT INTO [dbo].[tblInvoiceArticleUnit] ([Name], [CustomerId]) VALUES ('styck', @CustomerId)
SET @Styck = SCOPE_IDENTITY(); 

DECLARE @H INT
INSERT INTO [dbo].[tblInvoiceArticleUnit] ([Name], [CustomerId]) VALUES ('H', @CustomerId)
SET @H = SCOPE_IDENTITY(); 

DECLARE @StyckH INT
INSERT INTO [dbo].[tblInvoiceArticleUnit] ([Name], [CustomerId]) VALUES ('styck/H', @CustomerId)
SET @StyckH = SCOPE_IDENTITY(); 

DECLARE @AdPublication INT
DECLARE @InitalSelection INT
DECLARE @IntroductionPackage INT
DECLARE @RecruitmentStore INT
DECLARE @NewEmployeePackage INT

INSERT INTO [dbo].[tblInvoiceArticle] ([Number], [Name], [ProductAreaId], [CustomerId]) VALUES (5600, 'Test', @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([Number], [Name], [ProductAreaId], [CustomerId]) VALUES (5610, 'Ad publication', @Recruitment, @CustomerId)
SET @AdPublication = SCOPE_IDENTITY(); 
INSERT INTO [dbo].[tblInvoiceArticle] ([Number], [Name], [ProductAreaId], [CustomerId]) VALUES (5620, 'Initial selection/screening per 50 candidates', @Recruitment, @CustomerId)
SET @InitalSelection = SCOPE_IDENTITY(); 
INSERT INTO [dbo].[tblInvoiceArticle] ([Number], [Name], [ProductAreaId], [CustomerId]) VALUES (5630, 'Introduction package', @Recruitment, @CustomerId)
SET @IntroductionPackage = SCOPE_IDENTITY(); 
INSERT INTO [dbo].[tblInvoiceArticle] ([Number], [Name], [ProductAreaId], [CustomerId]) VALUES (5670, 'Recruitment Store opening', @Recruitment, @CustomerId)
SET @RecruitmentStore = SCOPE_IDENTITY(); 
INSERT INTO [dbo].[tblInvoiceArticle] ([Number], [Name], [ProductAreaId], [CustomerId]) VALUES (6010, 'New employee IT package', @NewEmployee, @CustomerId)
SET @NewEmployeePackage = SCOPE_IDENTITY(); 

INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@AdPublication, 5611, 'Ad publication - internal', @Styck, 1000, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@AdPublication, 5612, 'Ad publication - external', @Styck, 2220, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@AdPublication, 5613, 'Ad publication - Linked In', @Styck, 4420, @Recruitment, @CustomerId)

INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5621, 'Phone interview/first interview (45 min)', @Styck, 990, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5622, 'Face-to-face interview (1-1,5 h)', @Styck, 1330, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5623, 'PI Testning (incl. Feedback)', @Styck, 2750, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5624, 'PLI Testing (incl. Feedback)', @Styck, 315, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5625, 'OPQ testning (incl. Feedback)', @Styck, 3870, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5626, 'MQ testing (incl. Feedback)', @Styck, 1110, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5627, 'Verify proficiency testing (incl. Feedback)', @Styck, 1110, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@InitalSelection, 5628, 'Sending test links (exkl. feedback)', @Styck, 150, @Recruitment, @CustomerId)

INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5631, 'Recruitment training', @StyckH, 1000, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5632, 'Recruitment package 1', @Styck, 2430, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5633, 'Recruitment package 2', @Styck, 5250, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5634, 'Recruitment package 3', @Styck, 34150, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5635, 'Other recruitment services', @H, 665, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5640, 'Traveltime', NULL, 665, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5650, 'Hiring of recruitment specialist', NULL, 1000, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@IntroductionPackage, 5660, 'Summer recruitment', NULL, 18000, @Recruitment, @CustomerId)

INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5671, 'References', @H, 665, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5672, 'Talent bank', @H, 665, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5673, 'Assessment Centre', @Styck, 1000, @Recruitment, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5681, 'Restid', @H, 500, @LearningTraining, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5682, 'Trainer A', @H, 665, @LearningTraining, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5683, 'Trainer B', @H, 700, @LearningTraining, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5684, 'Trainer C', @H, 850, @LearningTraining, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5685, 'Training center', @Styck, 5000, @LearningTraining, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5686, 'Course fee', @Styck, 50000, @LearningTraining, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5691, '10 years of service - silver allen key', @Styck, 1500, @Celebration, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5692, 'Giftcard 25 years', @Styck, 500, @Celebration, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5693, 'Giftcard Retirement', @Styck, 1000, @Celebration, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5694, 'Giftcard - other celebrations', @Styck, 250, @Celebration, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5695, '50 years of service (golden allen key)', @Styck, 2000, @Celebration, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5696, '25 years of service - excl hotel', @Styck, 5000, @Celebration, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@RecruitmentStore, 5697, '25 years of service - excl hotel', @Styck, 5000, @Celebration, @CustomerId)

INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@NewEmployeePackage, 6011, 'Laptop', @Styck, 6000, @NewEmployee, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@NewEmployeePackage, 6012, 'Mobile', @Styck, 1000, @NewEmployee, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@NewEmployeePackage, 6013, 'Docking station', @Styck, 500, @NewEmployee, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@NewEmployeePackage, 6014, 'Computer bag', @Styck, 400, @NewEmployee, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@NewEmployeePackage, 6015, 'Screen', @Styck, 1200, @NewEmployee, @CustomerId)
INSERT INTO [dbo].[tblInvoiceArticle] ([ParentId], [Number], [Name], [UnitId], [Ppu], [ProductAreaId], [CustomerId]) 
VALUES (@NewEmployeePackage, 6016, 'Headset', @Styck, 250, @NewEmployee, @CustomerId)

