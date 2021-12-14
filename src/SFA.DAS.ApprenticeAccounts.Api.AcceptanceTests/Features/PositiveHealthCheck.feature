@database
@api

Feature: PositiveHealthCheck
	When the database online
	As a application monitor
	I want to be told the status of the api is healthy

Scenario: Ping returns a healthy state
	Given the api has started
	And the database is online
	When the ping endpoint is called
	Then the result should be ok

Scenario: Health returns a healthy status
	Given the database is online
	When the health endpoint is called
	Then the result should be healthy