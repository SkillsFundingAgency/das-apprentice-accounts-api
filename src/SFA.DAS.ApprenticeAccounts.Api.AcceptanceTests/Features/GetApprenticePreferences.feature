@database
@api

Feature: GetApprenticePreferences
	As a application user
	I want to retrieve valid apprentice preferences from the database

Scenario: Get the apprentice preferences for an apprentice
	Given there is an apprentice with preferences
	When we try to retrieve the apprentice preferences
	Then the result should return ok
	And the response should match the expected apprentice preferences values

Scenario: Get the apprentice preferences for an apprentice and a preference
	Given there is an apprentice with preferences
	When we try to retrieve the apprentice preference
	Then the result should return ok
	And the response should match the expected apprentice preference value
