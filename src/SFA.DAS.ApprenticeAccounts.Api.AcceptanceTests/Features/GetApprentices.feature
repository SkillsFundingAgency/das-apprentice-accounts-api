@database
@api

Feature: GetApprentices
	As an application user
	I want to retrieve apprentices from the database

Scenario: Get apprentices for sync by ids only
	Given there are apprentices with updates
	When we try to retrieve apprentices for sync by ids only
	Then the result should return ok
	And the response should contain the expected apprentice

Scenario: Get apprentices for sync by a date in the past
	Given there are apprentices with updates
	When we try to retrieve apprentices for sync by a past date
	Then the result should return ok
	And the response should contain the expected apprentice

Scenario: Get apprentices for sync by a date in the future
	Given there are apprentices with updates
	When we try to retrieve apprentices for sync by a future date
	Then the result should return ok
	And the response should have no apprentices

Scenario: Get apprentices for sync by empty parameters
	Given there are apprentices with updates
	When we try to retrieve apprentices for sync by empty parameters
	Then the result should return ok
	And the response should have no apprentices