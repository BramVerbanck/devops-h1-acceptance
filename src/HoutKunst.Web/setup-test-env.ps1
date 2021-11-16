<#
.NOTES
  Author:							Kenny Buffel
  Creation Date:  		21/10/2021
	Modification Date:	09/11/2021

.DESCRIPTION
  This script checks if the local setup is ready to deploy the test environment.
#>

<#
		VARIABLES
#>

$GithubRepoOps = "https://github.com/HoGentTIN/devops-project-ops-2122-h1-ops/blob/master"
$Resources = @(
	"Dockerfile"
	"docker-compose.yml"
	"test-app.ps1"
	"setup-test-env.ps1"
)
$GitIgnoreFile = "..\..\.gitignore"

<#
		FUNCTIONS
#>

function Add-ResourcesToGitIgnore($List) {
	$Header = "Test environment resource files"
	$GitIgnoreFileContentFound = (Select-String -Path $GitIgnoreFile -Pattern $Header).LineNumber -ne $null
	
	if (!$GitIgnoreFileContentFound) {
		"`n# $($Header)" | Add-Content $GitIgnoreFile
	
		foreach ($Resource in $List) {
			$Resource | Add-Content $GitIgnoreFile
		}
	}
}

function Check-DockerEngineIsRunning {
	$DockerEngineRunning = (docker ps 2> $null) -ne $null
	
	if (!($DockerEngineRunning)) {
		$DockerEngineInstallLocation = (Get-ChildItem HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall | % { Get-ItemProperty $_.PsPath } | Select DisplayName,InstallLocation | Where { $_.DisplayName -like "*docker*" }).InstallLocation
		start "$($DockerEngineInstallLocation)\Docker Desktop.exe"
		
		while (!($DockerEngineRunning)) {
			Start-Sleep -s 3
			$DockerEngineRunning = (docker ps 2> $null) -ne $null
		}
	}
}

function Check-PackageInstallation {
  $DockerInstalled = (docker --version) -ne $null
  $DockerComposeInstalled = (docker-compose --version) -ne $null

  Write-ListItem("Package 'docker' is installed...$(Convert-BoolToString($DockerInstalled))")
  Write-ListItem("Package 'docker-compose' is installed......$(Convert-BoolToString($DockerComposeInstalled))")

  if (!($DockerInstalled -AND $DockerComposeInstalled)) {
		$ErrorMessagePackages = "One or more packages are missing.`n"
		$ErrorMessagePackages = "Refer to the user manual on the Ops Github repository for more information on how to install them:`n`n"
		$ErrorMessagePackages = "https://github.com/HoGentTIN/devops-project-ops-2122-h1-ops/blob/master/test-environment/user-manual.md#docker-installation"
		Write-Error($ErrorMessagePackages)
	}
}

function Check-Resources($List) {
	
	foreach ($Resource in $List) {
		$ResourceFound = Test-Path -Path $pwd\$Resource
		Write-ListItem("Resource '$($Resource)' found...$(Convert-BoolToString($ResourceFound))")
		
		if (!($ResourceFound)) {
			$ErrorMessageResources = "One ore more resources are missing.`n"
			$ErrorMessageResources += "Refer to the `'test-environment/_resources` directory on the Ops Github repository to add them:`n`n"
			$ErrorMessageResources += "https://github.com/HoGentTIN/devops-project-ops-2122-h1-ops/blob/master/test-environment/_resources"
			Write-Error($ErrorMessageResources)
			
			break
		}
	}

}

function Check-ScriptInCorrectFolder() {
	$ScriptInCorrectFolder = Test-Path -Path $pwd\*.sln
	
	if (!$ScriptInCorrectFolder){
		$ErrorMessageScriptLocation = "This script seems to be in the wrong locaiton: no .NET '.sln' file was found in this folder."
		Write-Error($ErrorMessageScriptLocation)
	}
}

function Convert-BoolToString($Bool) {
  if ($Bool) {
    return "OK"
  } else {
    return "FAILED"
  }
}

function Test-BuildApp() {
	docker-compose build -q
}

function Validate-DockerComposeConfiguration() {
	docker-compose config -q
}

function Write-Error($Message) {
  Write-Host "`n$($Message)`n" -ForegroundColor Red -ErrorAction Stop
	exit
}

function Write-Info($Message) {
  Write-Host "`n   $($Message)`n" -ForegroundColor Yellow
}

function Write-ListItem($Line) {
	$Color = "White"
	if($Line -like '*FAILED*') {
		$Color = "Yellow"
	}
	
	Write-Host "   * $($Line)`n" -ForegroundColor "$Color"
}

function Write-Step($Step) {
  Write-Host "`n> $($Step)`n" -ForegroundColor Green
}

<#
		MAIN
#>

Write-Step("[1/7] Checking script location...")
Check-ScriptInCorrectFolder

Write-Step("[2/7] Checking packages...")
Check-PackageInstallation
		
Write-Step("[3/7] Checking resources...")
Check-Resources($Resources)

Write-Step("[4/7] Checking '.gitignore' file...")
Add-ResourcesToGitIgnore($Resources)

Write-Step("[5/7] Checking Docker Engine is running...")
Check-DockerEngineIsRunning

Write-Step("[6/7] Validating docker-compose.yml...")
Write-Info("If any message is shown below, contact an Ops member to check it.")
Validate-DockerComposeConfiguration
			
Write-Step("[7/7] Testing build application...")
Write-Info("If any message is shown below, contact an Ops member to check it.")
Test-BuildApp

# Keep Powershell script window open
cmd /c pause
