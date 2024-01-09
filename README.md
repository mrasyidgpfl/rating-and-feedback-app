# Rating and Feedback System

This is our repo for the eComindo Project: Rating and Feedback System. This project contains:
- C#
- .NET Core 2.1
- Azure Functions
- Angular JS

## How to develop

Terdapat dua direktori, yaitu backend yang berisi Azure Functions, dan frontend yang berisi Angular apps.

Di dalam direktori backend, terdapat 3 sub direktori, yaitu App, Test, dan Test.Helper. 
Sub direktori App digunakan untuk mengembangkan Azure Functions. 
Sub direktori Test digunakan untuk membuat unit test dari Azure Functions. 
Sub direktori Test.Helper berisi tools yang dibutuhkan untuk melakukan unit test.

Di dalam direktori frontend, terdapat Angular apps project. Silahkan develop di dalam sub direktori src.

## Installing Dependencies

dotnet : 
- Visual Studio : tinggal buka file .sln
- Visual Studio Code / Notepad / CLI : https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-function-vs-code
    - dokumentasi dependency : https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#v2
    - Install dotnet sdk v2
    - Install nodejs sama npm
    - Install azure functions core : `npm install -g azure-functions-core-tools`

angular : https://angular.io/guide/quickstart

## Pre-running apps

backend : setting env variable => `func settings add FUNCTIONS_WORKER_RUNTIME dotnet`

frontend : 
- `cd frontend`
- `npm install`

## How to run

backend : run menggunakan Visual Studio atau `func host start --build` (menggunakan azure functions cli)

frontend : `ng serve --open`

## How to test

backend : `dotnet test /p:CollectCoverage=true`

frontend : `npm run test` & `npm run e2e`


