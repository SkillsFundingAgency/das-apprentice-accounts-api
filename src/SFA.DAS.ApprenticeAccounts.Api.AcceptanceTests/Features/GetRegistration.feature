@database
@api

Feature: GetRegistration
	As a application user
	I want to retrieve registration details

Scenario: Getting a registration which does exist
	Given there is a registration with a First Viewed on <FirstViewedOn> and has the <UserIdentityId> assigned to it
	When we try to retrieve the registration
	Then the result should return ok
	And the response should match the registration in the database with <ExpectedHasViewedVerificationValue> and <ExpectedHasCompletedVerificationValue>

Examples:
	| FirstViewedOn | UserIdentityId                       | ExpectedHasViewedVerificationValue | ExpectedHasCompletedVerificationValue |
	| 2021-01-01    | 15AC916D-C20A-4521-B7D5-07DB472C26BF | true                               | true                                  |
	|               | 15AC916D-C20A-4521-B7D5-07DB472C26BF | false                              | true                                  |
	| 2021-02-01    |                                      | true                               | false                                 |
	|               |                                      | false                              | false                                 |

Scenario: Trying to get a registration which does NOT exist
	Given there is no registration
	When we try to retrieve the registration
	Then the result should return not found

Scenario: Trying to get a registration with an invalid Id
	When we try to retrieve the registration using a bad request format
	Then the result should return bad request

Scenario: Trying to get a registration with an empty Id
	Given there is an empty apprentice id
	When we try to retrieve the registration
	Then the result should return bad request
	And the error must be say apprentice id must be valid
