@database
@api
Feature: ConfirmEmployer
	As an apprentice I want to be able to confirm my employer

Scenario: Positively confirm an employer
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmEmployerRequest stating the employer is correct
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Negatively confirm an employer
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmEmployerRequest stating the employer is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Attempt to change an employer confirmirmation
	Given we have an apprenticeship that has previously had its employer positively confirmed
	And a ConfirmEmployerRequest stating the employer is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

