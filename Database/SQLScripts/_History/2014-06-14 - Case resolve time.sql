CREATE PROCEDURE dbo.dhCalculateCaseResolveTime
	@CustomerId INT = NULL,
	@CaseId INT = NULL
AS

SELECT 
	r.CaseId AS CaseId,
	r.CaseNumber AS CaseNumber,
	r.RegistrationDate AS RegistrationDate,
	r.CaseResolveTimeUTC AS CaseResolveTimeUTC,
	r.FinishingDate AS FinishingDate,
	r.StateSecondary AS StateSecondary,
	r.CustomerId AS CustomerId
FROM
(SELECT 
	c.Id AS CaseId,
	c.Casenumber AS CaseNumber,
	c.RegTime AS RegistrationDate,
	ch.CreatedDate AS CaseResolveTimeUTC,
	ch.FinishingDate AS FinishingDate,
	ss.StateSecondary AS StateSecondary,
	c.Customer_Id AS CustomerId,
	ROW_NUMBER() OVER (PARTITION BY c.Id ORDER BY ch.CreatedDate DESC) AS RowNumber
FROM dbo.tblCase c 
LEFT JOIN dbo.tblCaseHistory ch ON ch.Case_Id = c.Id
LEFT JOIN dbo.tblStateSecondary ss ON ss.Id = ch.StateSecondary_Id
WHERE 
(ch.FinishingDate IS NOT NULL OR ss.IncludeInCaseStatistics = 0)
AND (@CustomerId IS NULL OR c.Customer_Id = @CustomerId)
AND (@CaseId IS NULL OR c.Id = @CaseId)
) AS r
WHERE r.RowNumber = 1
ORDER BY r.CaseId

GO
