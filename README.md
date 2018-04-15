# TwitterSimulator
This is a C# .net 4.6.2 solution.
The application reads txt files and produces a twitter feed on console


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

Visual Studio 2017
.net 4.6.2

### Installing

Clone the repositiory and open the solution file .\src\TwitterSimulator.sln
Restore all depenedencies by right clicking the solution in VS and select "Restore NuGet Packages"
You can then build the application and run "F5"
The application will use the data in the folder .\src\TwitterSimulator\SampleData\user.txt and .\src\TwitterSimulator\SampleData\tweet.txt

## Running the tests

All tests wre created using MS test so you can run these tests in VS using Test Explorer


## Deployment

Execute a Release build and copy all binaries into the deployment location.
Launch the TwitterSimulator.exe file.
This application will look for the configuration settingsin the file app.config in the location .\src\TwitterSimulator\app.config.
When the solutuion is built the file will be renamed to TwitterSimulator.exe.config and placed alongside the binaries.
Pleasebe sure to copy this to the deployment location and update if required.

## Built With

Visual Studio
MsBuild

## Authors

* Zubair Sukan* 


