@database
@api
Feature: RegistrationReminderSent
	Once a remainer has been sent for a person who has not yet signed up, mark the registration record

Scenario: Registration Reminder is sent the first time
	Given the apprentice has not been sent a reminder
	When we receive a request to say reminder has been sent
	Then the response is OK
	And the registration record is updated

Scenario: Registration Reminder is sent a second time
	Given the apprentice has already been sent a reminder
	When we receive a request to say reminder has been sent
	Then the response is OK
	And the registration record is not updated
