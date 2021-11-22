CREATE VIEW DashboardReporting.ApprenticeshipDashboardView
AS
SELECT
	Id,
	ApprenticeId,
	CreatedOn,
	LastViewed
FROM
	dbo.Apprenticeship
GO