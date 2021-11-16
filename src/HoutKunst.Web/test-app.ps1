<#
.NOTES
  Author:							Kenny Buffel
  Creation Date:  		15/10/2021
	Modification Date:	14/11/2021

.DESCRIPTION
  This scripts deploys the test environment.
#>

<#
		VARIABLES
#>

$ProjectFolder = ".\HoutKunst.Web\Server\"
$NuGetPackages = @(
	"Npgsql.EntityFrameworkCore.PostgreSQL"
	"Npgsql.EntityFrameworkCore.PostgreSQL.Design"
)
$DotNetTools = @(
	"dotnet-ef"
)
$DotNetUserSecrets = @(
	"Kestrel:Certificates:Default:Path=/root/.aspnet/https/Houtkunst.Web.Server.pfx"
	"Kestrel:Certificates:Default:Password=D3v0psD0tN3tH1"
	"ConnectionStrings:DefaultConnection=Host=localhost;Database=houtkunst;Username=hout;Password=kunst"
	"ConnectionStrings:AzureBlobStorage=DefaultEndpointsProtocol=https;AccountName=houtkunsth1;AccountKey=1nLubqACN9PahT+ZqOx8II4zq3xXdi68LW2aHXO/Le9buresKVMCnxVY4++N/QkOppSmtxAcXUBQdZUghKF/Mg==;EndpointSuffix=core.windows.net"
)
$DiscordWebhook = "https://discord.com/api/webhooks/901760790900637727/PXUJDItg459t8xtc30u7Q34VKDcsqOsDFiKsAFhNNzTAD7svXE2pTlEeV5Ao3BPUL0Yc"

<#
		FUNCTIONS
#>

function Check-Certificate() {
	$CertificateFound = (dotnet dev-certs https -c) -eq $null
	
	if (!($CertificateFound)) {
		dotnet dev-certs https -ep $env:AppData\ASP.NET\Https\Houtkunst.Web.Server.pfx -p ($DotNetUserSecrets[1] -split "=",2)[1] | Out-Null
	}

	dotnet dev-certs https -t -q | Out-Null
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

function Check-NuGetPackages($List) {
	dotnet restore | Out-Null
	
	foreach ($NuGetPackage in $List) {
			$NuGetPackageFound = (dotnet list $ProjectFolder package | Select-String $NuGetPackage) -ne $null
			Write-ListItem("NuGet package '$($NuGetPackage)' found...$(Convert-BoolToString($NuGetPackageFound))")
		
			if (!($NuGetPackageFound)) {
				Write-Info("Adding...")
				dotnet add $ProjectFolder package $NuGetPackage | Out-Null
			}
		}
}

function Check-DotNetTools($List) {
	foreach ($Tool in $List) {
			$DotNetToolFound = (dotnet tool list --global | Select-String $Tool) -ne $null
			Write-ListItem("Dotnet tool '$($Tool)' found...$(Convert-BoolToString($DotNetToolFound))")
		
			if (!($DotNetToolFound)) {
				Write-Info("Adding...")
				dotnet tool install --global $Tool | Out-Null
			}
	}
}

function Check-DotNetUserSecrets($List) {
	foreach ($UserSecret in $List) {
		$UserSecretParameter = ($UserSecret -split '=',2)[0]
		$UserSecretValue = ($UserSecret -split '=',2)[1]
		
		$UserSecretFound = (dotnet user-secrets list -p $ProjectFolder | Select-String $UserSecretParameter | Measure-Object).Count -eq 1
		Write-ListItem("User secret '$($UserSecretParameter)' found...$(Convert-BoolToString($UserSecretFound))")
			
		if (!($UserSecretFound)) {
			Write-Info("Adding...")
			dotnet user-secrets set $UserSecretParameter $UserSecretValue -p $ProjectFolder | Out-Null
		}
	}
}

function Convert-BoolToString($Bool) {
  if ($Bool) {
    return "OK"
  } else {
    return "FAILED"
  }
}

function Deploy-TestEnvironment() {
	Check-DockerEngineIsRunning
	
	docker-compose down
	docker-compose up -d --build
}

function Push-MessageToDiscordChannel($Message) {
	Write-Info("Errors found, pushing to Discord.")
	Write-Info("If the error message does not appear in Discord, Your anti-virus software may be blocking the request.")
	
	Send-DiscordMessage -URL $DiscordWebhook -Message $Message
	exit
}

function Show-DockerComposeInfo() {
	docker-compose ps
	Write-Info("`n")
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

function Update-Database() {
	Write-ListItem("Updating database schema...")
	dotnet ef database drop -p $ProjectFolder -f --no-build 1> $null
	dotnet ef migrations remove -p $ProjectFolder --no-build 1> $null
	dotnet ef migrations add HoutKunstSchema -p $ProjectFolder 1> $null
	
	Write-ListItem("Applying schema to database...")
	dotnet ef database update -p $ProjectFolder 1> $null
}

<#
		MAIN
#>

Write-Step("[1/6] Checking NuGet packages...")
Check-NuGetPackages($NuGetPackages)

Write-Step("[2/6] Checking .NET user secrets...")
Check-DotNetUserSecrets($DotNetUserSecrets)

Write-Step("[3/6] Checking dotnet tools...")
Check-DotNetTools($DotNetTools)

Write-Step("[4/6] Checking HTTPS certificate...")
Check-Certificate

Write-Step("[5/6] Deploying test environment...")
Deploy-TestEnvironment

Write-Step("[6/6] Provisioning database...")
Update-Database

Write-Step("Test environment information")
Show-DockerComposeInfo

# Keep Powershell script window open
cmd /c pause
