@database
@api

Feature: ConfirmationCountdown
	As an apprentice
	I need to know how long I have to complete the journey
	So that I complete the journey before the timer runs-out

Scenario: Countdown greater than zero
	Given we have an existing apprenticeship that was approved on <Approved On>
	When retrieving the apprenticeship
	Then the response should contain confirmation dealine <Confirm Before>

	Examples: 
	| Approved On      | Confirm Before   |
	| 2021-01-01       | 2021-01-15       |
	| 2021-01-30       | 2021-02-13       |
	| 2021-03-12 10:59 | 2021-03-26 10:59 |

Scenario: Change of circumstances
	Given we have an existing apprenticeship that was approved on <Approved On>
	When we have received a change of circumstances that was approved on <CoC Approved On>
	And retrieving the apprenticeship
	Then the response should contain confirmation dealine <Confirm Before>

	Examples: 
	| Approved On      | CoC Approved On  | Confirm Before   |
	| 2021-01-01       | 2021-01-05       | 2021-01-19       |
	| 2021-01-30       | 2021-02-15       | 2021-03-01       |
