@api

Feature: NegativeHealthCheck
	When the database is not on line
	As a application monitor
	I want to be told the status of the api is unhealthy

Scenario: Ping returns an unhealthy state
	Given the api has started
	And the database is offline
	When the ping endpoint is called
	Then the result should not be healthy

Scenario: Health return an unhealthy status
	Given the database is offline
	When the health endpoint is called
	Then the result should not be healthy