Param(
    [string]$version,
    [string]$server, 
    [string]$internalServer, 
    [string]$apiKey
)

### Configuration

$packages = @(
	[pscustomobject]@{ Package = "Riganti.Selenium.AssertApi" },
	[pscustomobject]@{ Package = "Riganti.Selenium.Coordinator.Client" },
	[pscustomobject]@{ Package = "Riganti.Selenium.Core" },
	[pscustomobject]@{ Package = "Riganti.Selenium.Core.Abstractions" },
	[pscustomobject]@{ Package = "Riganti.Selenium.DotVVM" },
	[pscustomobject]@{ Package = "Riganti.Selenium.FluentApi" },
	[pscustomobject]@{ Package = "Riganti.Selenium.LambdaApi" },
	[pscustomobject]@{ Package = "Riganti.Selenium.MSTest2Integration" },
	[pscustomobject]@{ Package = "Riganti.Selenium.Validators" },
	[pscustomobject]@{ Package = "Riganti.Selenium.xUnitIntegration" }
)

foreach($package in $packages){

    $packageId = $package.Package
    $webClient = New-Object System.Net.WebClient
   
    & .\tools\nuget.exe install $packageId -OutputDirectory .\tools\packages -version $version -DirectDownload -NoCache -DependencyVersion Ignore -source $internalServer
    $nupkgFile = dir -s ./tools/packages/$packageId.$version.nupkg | Select -First 1
    Write-Host "Downloaded package located on '$nupkgFile'"

    Write-Host "Uploading package..."
    & .\Tools\nuget.exe push $nupkgFile -source $server -apiKey $apiKey
    Write-Host "Package uploaded to $server."

    Remove-Item $nupkgFile
}
