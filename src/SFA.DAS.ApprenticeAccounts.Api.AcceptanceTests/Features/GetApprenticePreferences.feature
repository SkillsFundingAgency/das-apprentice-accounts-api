﻿@database
@api

Feature: GetApprenticePreferences
	As a application user
	I want to retrieve valid apprentice preferences from the database

Scenario: Get the apprentice preferences for an apprentice
	Given there is an apprentice Id
	When we try to retrieve the apprentice preferences
	Then the result should return ok
	And the response should match the expected apprentice preference values

Scenario: Get the apprentice preferences for an apprentice and a preference
	Given there is an apprentice Id and a preference Id
	When we try to retrieve the apprentice preferences
	Then the result should return ok
	And the response should match the expected apprentice preference values
