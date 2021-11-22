@database
@api
Feature: HowApprenticeshipWillBeDelivered
	As an apprentice I want to be able to confirm that I have read and understood 
	the HowMyApprenticeshipWillBeDelivered page

Scenario: Positively confirm that I understood the HowMyApprenticeshipWillBeDelivered page
	Given we have an apprenticeship waiting to be confirmed
	And a HowApprenticeshipWillBeDeliveredRequest stating the HowMyApprenticeshipWillBeDelivered is correct
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

	Scenario: Negatively confirm that I haven't understood the HowMyApprenticeshipWillBeDelivered page
	Given we have an apprenticeship waiting to be confirmed
	And a HowApprenticeshipWillBeDeliveredRequest stating the HowApprenticeshipWillBeDeliveredRequest is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated

Scenario: Attempt to change a confirmation selection
	Given we have an apprenticeship that has previously had HowMyApprenticeshipWillBeDelivered positively confirmed
	And a HowApprenticeshipWillBeDeliveredRequest stating the HowApprenticeshipWillBeDeliveredRequest is incorrect
	When we send the confirmation
	Then the response is OK
	And the apprenticeship record is updated
