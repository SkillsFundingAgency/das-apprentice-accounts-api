## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _Project Name_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

_Update these badges with the correct information for this project. These give the status of the project at a glance and also sign-post developers to the appropriate resources they will need to get up and running_

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/_projectname_?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=_projectid_&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?rapidView=564&projectKey=_projectKey_)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/_pageurl_)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

The Apprentice Accounts API provides basic account information for an apprentice, that may be consumed by the various services that comprise the overall Apprentice Portal.  It is the _Inner API_ tier, following the [Apprenticeship Service's technical guidance](https://skillsfundingagency.github.io/das-technical-guidance/api_standards/1-api-patterns-as#api-patterns-within-the-as).

## How It Works

This repo has the api endpoints to perform crud operaions on account entity (the actual apprentice – its its own microservice). 

## 🚀 Installation

The repo has a database project which should be deployed locally to a database named ‘SFA.DAS.ApprenticeLoginService.Database’

### Developer Setup

runs on: https://localhost:5801/index.html

The Apprentice Accounts API is a standard ASP.Net Core Web API project using a SQL Server database.

also see onboarding guide [here](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3518529551/Apprentice+Portal+-+on+boarding+guide)

### ⏭️ Pre-Requisites

_Add the pre-requisites needed to successfully run the project so that new developers know how they are to setup their development environment_

* A clone of this repository
* A code editor that supports .NetCore 3.1
* SQL Server

### 🔧 Config

This API uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config).

* A connection string for the database
* Add an appsettings.Development.json file
    * Add your SQL connection strings to the relevant sections of the file
* Publish the ApprenticeAccounts.Database project.

AppSettings.Development.json file
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true",
  "ConfigNames": "SFA.DAS.ApprenticeAccounts.Api",
  "EnvironmentName": "LOCAL",
  "Version": "1.0",
  "ApplicationSettings": {
    "DbConnectionString": "Data Source=.;Initial Catalog=SFA.DAS.ApprenticeAccounts.Database;Integrated Security=True;Pooling=False;Connect Timeout=30"
  }
}
```

## 🔗 External Dependencies

None

## 💽 Technologies

_List the key technologies in-use in the project. This will give an indication as to the skill set required to understand and contribute to the project_

```
* .NetCore 3.1
* NLog
* NUnit
* Moq
* FluentAssertions
```

## 🐛 Known Issues

None