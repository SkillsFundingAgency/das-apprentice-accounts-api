@database
@api

Feature: GetRegistrationsReminders
	As a application user
	I want to return registrations for people who haven't yet signed up and at least 'started the identification process' and who haven't already had a reminder sent. 
	Note:	If they view the record, they must have signed in
			When they complete the record, the User Identity Id is recorded

Background:
	Given the following registration details exist
	| Email           | FirstName | LastName  | Created On | First Viewed On | Sign Up Reminder Sent On | User Identity Id                     |
	| Alexa@Armstrong | Alexa     | Armstrong | 2021-01-01 | 2021-01-10      |                          |                                      |
	| Neil@Armstrong  | Neil      | Armstrong | 2021-01-01 |                 |                          |                                      |
	| Andy@Scott      | Andy      | Scott     | 2020-12-17 | 2021-01-10      | 2020-12-30               |                                      |
	| Bill@Peterson   | Bill      | Peterson  | 2021-01-16 |                 |                          |                                      |
	| Micheal@Pain    | Micheal   | Pain      | 2021-01-01 | 2021-01-03      |                          | 17341578-F7C9-4D65-8208-DE630E31C2C3 |

Scenario: When query is run on 2021-01-02
	When we want reminders before cut off date 2021-01-02
	Then the result should return 1 matching registration
	And there should be a registration with the email Neil@Armstrong and it's expected values 

Scenario: When query is run on 2021-01-17
	When we want reminders before cut off date 2021-01-17
	Then the result should return 2 matching registration
	And there should be a registration with the email Neil@Armstrong and it's expected values 
	And there should be a registration with the email Bill@Peterson and it's expected values 

Scenario: When query is run on 2020-12-30
	When we want reminders before cut off date 2020-12-30
	Then the result should return 0 matching registration