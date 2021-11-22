BEGIN TRANSACTION 

UPDATE Revision SET
	LastViewed = A.LastViewed
FROM Revision R
LEFT JOIN Apprenticeship A ON A.Id = R.ApprenticeshipId 
WHERE R.LastViewed IS NULL AND A.LastViewed IS NOT NULL AND A.LastViewed > R.CommitmentsApprovedOn

UPDATE Apprenticeship SET LastViewed = NULL WHERE LastViewed IS NOT NULL

COMMIT TRANSACTION