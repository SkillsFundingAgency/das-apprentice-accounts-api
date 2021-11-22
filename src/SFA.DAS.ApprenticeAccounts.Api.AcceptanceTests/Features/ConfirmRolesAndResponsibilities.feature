@database
@api
Feature: ConfirmRolesAndResponsibilities
	As an apprentice I want to be able to confirm my roles and responsibilities

Scenario: Positively confirm roles and responsibilities
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmRolesAndResponsibilitiesRequest stating the roles and responsibilities are correct
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Negatively confirm an employer
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmRolesAndResponsibilitiesRequest stating the roles and responsibilities are incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Attempt to change an employer confirmirmation
	Given we have an apprenticeship that has previously had its roles and responsibilities confirmed
	And a ConfirmRolesAndResponsibilitiesRequest stating the roles and responsibilities are incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated
