@database
@api
Feature: ConfirmApprenticeship
	As an apprentice I want to be able to confirm my apprenticeship

Scenario: Positively confirm apprenticeship
	Given we have an apprenticeship waiting to be confirmed
	And a ConfirmApprenticeshipRequest stating the apprenticeship is confirmed
	When the apprentice confirms the apprenticeship
	Then the response is OK
	And the apprenticeship record is updated to show confirmed
	And the Confirmation Confirmed event is published

Scenario: Attempt to confirm apprenticeship prematurely
	Given we have an apprenticeship not ready to be confirmed
	And a ConfirmApprenticeshipRequest stating the apprenticeship is confirmed
	When the apprentice confirms the apprenticeship
	Then the response is BadRequest
	And the apprenticeship record is untouched
