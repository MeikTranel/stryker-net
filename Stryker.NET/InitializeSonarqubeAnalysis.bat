REM Copy targets to correct path until runner is updated.
mkdir "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Microsoft.Common.targets\ImportBefore"
copy packages\MSBuild.SonarQube.Runner.Tool.1.0.0\tools\Targets\SonarQube.Integration.ImportBefore.targets "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Microsoft.Common.targets\ImportBefore\"

echo Sonarqube analysis for: %APPVEYOR_REPO_COMMIT%

IF DEFINED APPVEYOR_PULL_REQUEST_NUMBER (
	echo Analysing Pull Request %APPVEYOR_PULL_REQUEST_NUMBER%
	@echo off
	packages\MSBuild.SonarQube.Runner.Tool.1.0.0\tools\SonarQube.Scanner.MSBuild.exe begin /k:"Stryker.NET" /d:"sonar.host.url=https://sonarqube.com" /v:"%APPVEYOR_REPO_COMMIT%" /d:"sonar.cs.dotcover.reportsPaths=DotCoverTestCoverageReport.html" /d:"sonar.cs.vstest.reportsPaths=%APPVEYOR_BUILD_FOLDER%\Stryker.NET\TestResults.trx" /d:"sonar.login=%sonarqube_auth_token%" /d:"sonar.analysis.mode=preview" /d:"sonar.github.oauth=%github_oauth_token%" /d:"sonar.github.pullRequest=%APPVEYOR_PULL_REQUEST_NUMBER%" /d:"sonar.links.scm_dev=https://github.com/infosupport/stryker-net" /d:sonar.issuesReport.console.enable=true /d:"sonar.github.repository=infosupport/stryker-net" /d:sonar.verbose=true
) ELSE (
	echo Analysing commit %APPVEYOR_REPO_COMMIT%
	@echo off
	packages\MSBuild.SonarQube.Runner.Tool.1.0.0\tools\SonarQube.Scanner.MSBuild.exe begin /k:"Stryker.NET" /d:"sonar.host.url=https://sonarqube.com" /v:"%APPVEYOR_REPO_COMMIT%" /d:"sonar.cs.dotcover.reportsPaths=DotCoverTestCoverageReport.html" /d:"sonar.cs.vstest.reportsPaths=%APPVEYOR_BUILD_FOLDER%\Stryker.NET\TestResults.trx" /d:"sonar.login=%sonarqube_auth_token%"
)