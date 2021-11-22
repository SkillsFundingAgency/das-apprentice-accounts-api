@database
@api

Feature: GetApprenticeshipRevisions
	As a application user
	I want to create a valid apprenticeship in the database

Scenario: When the apprenticeship for a given apprentice exists
	Given the apprenticeship exists and it's associated with this apprentice
	When we try to retrieve the apprenticeship revisions
	Then the result should return ok
	And the response should match the expected apprenticeship values

Scenario: When the apprenticeship doesn't exist
	Given there is no apprenticeship
	When we try to retrieve the apprenticeship revisions
	Then the result should return NotFound

Scenario: When the apprenticeship exists, but not for this apprentice
	Given the apprenticeship exists, but it's associated with another apprentice
	When we try to retrieve the apprenticeship revisions
	Then the result should return NotFound

Scenario: When multiple apprenticeships for a given apprentice exists
	Given many apprenticeships exists and are associated with this apprentice
	When we try to retrieve the apprenticeship revisions
	Then the result should return ok
	And the response should match the expected apprenticeship values

Scenario: When an apprenticeship with multiple revisions for a given apprentice exists
	Given the apprenticeships exists, has many revisions, and is associated with this apprentice
	When we try to retrieve the apprenticeship revisions
	Then the result should return ok
	And the response should match the expected apprenticeship values
