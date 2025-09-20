# Sondor Problem Result
![Build Status](https://dev.azure.com/sondortechnology/Sondor%20Infrastructure/_apis/build/status%2Fsondor-dev.sondor-problem-result-package?repoName=sondor-dev%2Fsondor-logging-loki-package&branchName=master) ![NuGet Downloads](https://img.shields.io/nuget/dt/Sondor.ProblemResults)

The Sondor problem result package, sets up common base problem detail results used for building APIs and services. It supports both the legacy method of throwing custom exceptions and the result pattern to ensure a consistent but performant problem detail implementation.

## Install Options
Install via NuGet
```cli
Install-Package Sondor.ProblemResults
```
Install via .NET Core command line
```cli
dotnet add package Sondor.ProblemResults
```

## Getting started
Follow the package instructions [here](/Sondor.ProblemResults/Sondor.ProblemResults/README.md).