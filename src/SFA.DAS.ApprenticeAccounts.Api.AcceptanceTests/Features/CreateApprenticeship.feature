@database
@api

Feature: CreateApprenticeship
	As a application user
	I want to create a valid apprenticeship in the database

Scenario: Trying to create an apprenticeship with invalid values
	Given we have an invalid apprenticeship request
	When the apprenticeship is posted
	Then the result should return bad request
	And the content should contain error list

Scenario: Trying to create an apprenticeship with valid values
	Given we have a valid apprenticeship request
	When the apprenticeship is posted
	Then the result should return OK
	And the registration exists in database
	And the Confirmation Commenced event is published
