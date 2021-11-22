@database
@api
Feature: ConfirmApprenticeshipDetails
	As an apprentice I want to be able to confirm my apprenticeship details

Scenario: Positively confirm apprenticeship details
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmApprenticeshipDetailsRequest stating the training provider is correct
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Negatively confirm a training provider
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmApprenticeshipDetailsRequest stating the training provider is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Attempt to change a training provider confirmirmation
	Given we have an apprenticeship that has previously had its apprenticeship details positively confirmed
	And a ConfirmApprenticeshipDetailsRequest stating the training provider is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated
