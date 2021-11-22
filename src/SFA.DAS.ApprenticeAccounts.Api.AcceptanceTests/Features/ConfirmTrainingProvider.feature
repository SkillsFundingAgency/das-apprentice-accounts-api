@database
@api
Feature: ConfirmTrainingProvider
	As an apprentice I want to be able to confirm my training provider

Scenario: Positively confirm a training provider
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmTrainingProviderRequest stating the training provider is correct
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Negatively confirm a training provider
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmTrainingProviderRequest stating the training provider is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Attempt to change a training provider confirmirmation
	Given we have an apprenticeship that has previously had its training provider positively confirmed
	And a ConfirmTrainingProviderRequest stating the training provider is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated
