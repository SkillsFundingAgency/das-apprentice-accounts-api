@database
@api

Feature: GetApprentice
	As a application user
	I want to create a valid apprenticeship in the database

Scenario: Get the apprentice's record
	Given there is one apprentice
	When we try to retrieve the apprentice
	Then the result should return ok
	And the response should match the expected apprentice values

Scenario: Get a missing record
	Given there is no apprentice
	When we try to retrieve the apprentice
	Then the result should return Not Found
