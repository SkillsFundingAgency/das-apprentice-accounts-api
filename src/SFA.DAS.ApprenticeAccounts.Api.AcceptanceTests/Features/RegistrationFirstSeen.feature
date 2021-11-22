@database
@api
Feature: RegistrationFirstSeen
	Once a user has successfully logged into the system. This will record when they first see the confirm your identity flow
	Note: This is needed to be able to differentiate between users who have not not yet created an account and those who have not verified 
		their identity

Scenario: Registration details are seen for the first time
	Given this is the first time the apprentice has seen the identity flow
	When we receive a request to mark registration as been viewed
	Then the response is OK
	And the registration record is updated

Scenario: Registration details are not seen for the first time
	Given this is not the first time the apprentice has seen the identity flow
	When we receive a request to mark registration as been viewed
	Then the response is OK
	And the registration record is not updated
