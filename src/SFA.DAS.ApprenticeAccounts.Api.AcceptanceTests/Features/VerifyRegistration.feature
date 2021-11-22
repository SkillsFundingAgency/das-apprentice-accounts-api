@database
@api

Feature: VerifyRegistration
	As an application user
	I want to verify and complete an existing registration

Scenario: A registration account is created
	Given we have an existing registration
	And we have a matching account
	And the request matches registration details
	When we verify that registration
	Then the response is OK
	And an apprenticeship record is created
	And the registration has been marked as completed
	And the registration CreatedOn field is unchanged
	And the apprenticeship email address confirmed event is published
	And do not send a Change of Circumstance email to the user

Scenario: A registration is submitted with a different email
	Given we have an existing registration
	And we have an account with a non-matching email
	And the request is for the account
	When we verify that registration
	Then the response is OK
	And an apprenticeship record is created
	And the registration has been marked as completed
	And the registration CreatedOn field is unchanged
	And do not send a Change of Circumstance email to the user

Scenario: A registration is submitted with the wrong Date of birth
	Given we have an existing registration
	And we have an account with a non-matching date of birth
	And the request is for the account
	When we verify that registration
	Then a bad request is returned
	And an identity mismatch domain error is returned

Scenario: A registration is re-submitted
	Given we have an existing already verified registration
	And the request is for a different account
	When we verify that registration
	Then a bad request is returned
	And an 'already verified' domain error is returned

Scenario: A registration is submitted with invalid values
	Given the verify registration request is invalid
	When we verify that registration
	Then a bad request is returned
	And response contains the expected error messages

Scenario: A registration is submitted which does not exist
	Given we do NOT have an existing registration
	And a valid registration request is submitted
	When we verify that registration
	Then a bad request is returned
	And response contains the not found error message