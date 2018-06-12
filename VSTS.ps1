$version = "0.1.1"

if($env:BUILD_SOURCEBRANCHNAME -and -not $env:BUILD_SOURCEBRANCHNAME -eq "master")
{
    $version = "$version."+$env:BUILD_BUILDNUMBER+"-"+$env:BUILD_SOURCEBRANCHNAME
}
Write-Host "##vso[build.updatebuildnumber]$version"
& .\build.ps1