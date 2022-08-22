@database
@api

Feature: GetPreferences
	As a application user
	I want to retrieve preferences from the database

Scenario: Get the preferences
	Given there are preferences
	When we try to retrieve the preferences
	Then the result should return ok
	And the response should match the expected preference values
