# Rating and Feedback App

This is our repository for a school project: Rating and Feedback System web application. This project contains:
- C#
- .NET Core 2.1
- Azure Functions
- Angular JS

## About the repository

There are two directories, namely the backend which contains Azure Functions, and the frontend which contains Angular apps.

In the backend directory, there are 3 sub-directories, namely App, Test, and Test.Helper. The App subdirectory is used to develop Azure Functions. The Test subdirectory is used to create unit tests of Azure Functions. The Helper.Test subdirectory contains the tools needed to perform unit tests.

In the frontend directory, there is an Angular app project.

## Installing Dependencies

dotnet: 
- Visual Studio: open file .sln
- Visual Studio Code / Notepad / CLI: https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function-vs-code
    - Dependencies: https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#v2
    - Install dotnet sdk v2
    - Install nodejs and npm
    - Install Azure functions core: `npm install -g azure-functions-core-tools`

angular: https://angular.io/guide/quickstart

## Pre-running apps

backend : setting env variable => `func settings add FUNCTIONS_WORKER_RUNTIME dotnet`

frontend : 
- `cd frontend`
- `npm install`

## How to run

backend: run using Visual Studio or `func host start --build` (using the Azure Functions CLI)

frontend: `ng serve --open`

## How to test

backend: `dotnet test /p:CollectCoverage=true`

frontend: `npm run test` & `npm run e2e`
