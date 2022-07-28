@database
@api

Feature: GetApprenticePreferences
	As a application user
	I want to create a valid apprenticeship in the database

Scenario: Get the apprentice preferences
	Given there is one apprentice
	When we try to retrieve the apprentice preferences
	Then the result should return ok
	And the response should match the expected apprentice preference values

Scenario: Get a missing record
	Given there is no apprentice
	When we try to retrieve the apprentice preferences
	Then the result should return Not Found
