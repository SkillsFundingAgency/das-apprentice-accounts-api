@database
@api
Feature: ChangeEmailAddress
	As an apprentice
	I want to be able to change the email address associated with my digital account
	So that I can still access my commitment & receive updates from the service

Scenario: Change to a valid email address
	Given we have an existing apprentice
	And a ChangeEmailCommand with a valid email address
	When we change the apprentice's email address
	Then the apprentice record is updated
	Then the UpdatedOn property is updated to today
	And the change history is recorded

Scenario: Reject change to an invalid email address
	Given we have an existing apprentice
	And a ChangeEmailCommand with an invalid email address
	When we change the apprentice's email address
	Then the apprentice record is not updated

Scenario: Ignore change to an existing email address
	Given we have an existing apprentice
	And a ChangeEmailCommand with the current email address
	When we change the apprentice's email address
	Then the apprentice record is not updated excluding updated on date
	But the UpdatedOn property is updated to today