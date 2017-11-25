[![Build status](https://ci.appveyor.com/api/projects/status/caflq6c3yvuqrklb/branch/master?svg=true)](https://ci.appveyor.com/project/simondel/stryker-net/branch/master)
[![Quality Gate](https://sonarqube.com/api/badges/gate?key=Stryker.NET)](https://sonarqube.com/dashboard/index/Stryker.NET)

![Stryker](stryker-80x80.png)

# Stryker.NET
*Professor X: For someone who hates mutants... you certainly keep some strange company.*  
*William Stryker: Oh, they serve their purpose... as long as they can be controlled.*

## Getting started
Stryker.NET offers you mutation testing for your .NET projects. It allows you to test your tests by temporarily inserting bugs.

The project is still in its early days and is not ready to use on your applications.

To run Striker-net you need to clone this repo, publish the project "Striker.NET" to %USERPROFILE%\AppData\Local\Microsoft\WindowsApps. 
From now on you can run  the command 'dotnet mutate' on the directory from where you want to run striker. This directory needs to contain you source code and your unit tests.

Want to help develop Stryker.NET? Check out our [contributing guide](/CONTRIBUTING.md)!
In the meantime, start by [mutation testing your JavaScript](https://stryker-mutator.github.io)

## Supported Mutators
Right now, Stryker.NET supports the following mutators:

### BinaryExpressionMutator
| Original | Mutated  |
| -------- | -------- |
| a + b    | a - b    |
| a - b    | a + b    |
| a * b    | a / b    |
| a / b    | a * b    |
| a % b    | a * b    |
| a < b    | a <= b   |
| a < b    | a >= b   |
| a > b    | a <= b   |
| a > b    | a >= b   |
| a <= b   | a < b    |
| a <= b   | a > b    |
| a >= b   | a < b    |
| a >= b   | a < b    |
| a == b   | a != b   |
| a != b   | a == b   |
| a \|\| b   | a && b   |
| a && b   | a \|\| b   |
