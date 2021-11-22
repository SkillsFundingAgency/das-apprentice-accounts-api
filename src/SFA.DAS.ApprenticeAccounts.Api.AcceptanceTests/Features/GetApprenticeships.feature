@database
@api

Feature: GetApprenticeships
	As a application user
	I want to create a valid apprenticeship in the database

Scenario: Get the list of apprentice's apprenticeships
	Given there is one apprenticeship
	When we try to retrieve the apprenticeships
	Then the result should return ok
	And the response should match the apprenticeship in the database

Scenario: Trying to create an apprenticeship with valid values
	Given there are no apprenticeships
	When we try to retrieve the apprenticeships
	Then the result should return ok
	And the response be an empty list
