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

Scenario: The apprentice record terms of service is set correctly for beta users
	Given there is one beta user apprentice
	And that apprentice has accepted the terms of service
	And there is a new version of terms of service released
	When we try to retrieve the apprentice
	Then the result should return ok
	And the response terms of service should be set correctly