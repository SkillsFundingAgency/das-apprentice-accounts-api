@database
@api

Feature: ChangeApprenticeship

Scenario: Update an apprenticeship with a new revision
	Given we have an existing apprenticeship
	And we have an update apprenticeship request
	When the update is posted
	Then the result should return OK
	And the new revision exists in database
	And the new revision has same commitments apprenticeship Id
	And the Confirmation Commenced event is published

Scenario: A new apprenticeship has been created as a continuation of an apprenticeship
	Given we have an existing apprenticeship
	And we have a update apprenticeship continuation request
	When the update is posted
	Then the result should return OK
	And the new revision exists in database
	And the new revision has a new commitments apprenticeship Id

Scenario: Update registration details if the apprentice has not confirmed his identity
	Given we do have an unconfirmed registration
	And we have an update apprenticeship request
	When the update is posted
	Then there should be no revisions in the database
	And we have updated the apprenticeship details for the unconfirmed registration
	And registration commitments apprenticeship are updated correctly

Scenario: Update registration details with Continuation Id if the apprentice has not confirmed his identity 
	Given we do have an unconfirmed registration
	And we have a update apprenticeship continuation request
	When the update is posted
	Then there should be no revisions in the database
	And we have updated the apprenticeship details for the unconfirmed registration
	And registration commitments apprenticeship are updated correctly

Scenario: No apprenticeship or registation details found
	Given we do not have an existing apprenticeship, confirmed or unconfirmed
	And we have an update apprenticeship request
	When the update is posted
	Then the result should return OK
	And a new registration record should exist with the correct information
	And send a Apprenticeship Registered Event

Scenario: No apprenticeship exist, but registation details are marked as completed
	Given we do not have an existing apprenticeship
	And we do have a verified registration 
	And we have an update apprenticeship request
	When the update is posted
	Then the response is bad request
	And a domain exception is thrown: "Cannot update registration as user has confirmed their identity"

Scenario: Do not update an apprenticeship when no consequential change is made
	Given we have an existing apprenticeship
	And we have an update apprenticeship request with no material change
	When the update is posted
	Then the result should return OK
	Then there should only be the original revision in the database
	And no Confirmation Commenced event is published

Scenario: Notify user of change
	Given we have an existing apprenticeship
	And we have an update apprenticeship request
	When the update is posted
	Then send a Change of Circumstance email to the user

Scenario: Trying to update apprenticeship which doesn't have an email
	Given we do not have an existing apprenticeship, confirmed or unconfirmed
	And we have an update apprenticeship request without an email
	When the update is posted
	Then the response is bad request
	And a validation exception is thrown for the field: "Email" 