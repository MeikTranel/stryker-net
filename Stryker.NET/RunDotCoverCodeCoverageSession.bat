packages\JetBrains.dotCover.CommandLineTools.2016.3.20170126.124346\tools\dotcover analyse /TargetExecutable="c:\Program Files\dotnet\dotnet.exe" /TargetArguments="test %APPVEYOR_BUILD_FOLDER%\Stryker.NET\Stryker.NET.UnitTests\Stryker.NET.UnitTests.csproj --logger \"trx;LogFileName=%APPVEYOR_BUILD_FOLDER%\Stryker.NET\TestResults.trx\" -o %APPVEYOR_BUILD_FOLDER%\Stryker.NET\Stryker.NET.UnitTests\bin\Release\netcoreapp1.1 --no-build" /Output=DotCoverTestCoverageReport.html /ReportType=HTML