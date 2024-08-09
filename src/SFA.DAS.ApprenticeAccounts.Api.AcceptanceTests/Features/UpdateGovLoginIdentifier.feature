@database
@api
Feature: UpdateGovIdentifier
  As an apprentice
  I want to be able update my gov identifier
  So that I can authenticate with gov one login
  
Scenario: Add gov login
  Given we have an existing apprentice
  And we Update gov login identifier with an valid value
  When we add the apprentice's GOV login identifier
  Then the apprentice record GOV login identifier is updated