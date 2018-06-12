$version = "0.1.1"

if(-not $env:BUILD_SOURCEBRANCHNAME -eq "master")
{
    $version = "$version."+$env:BUILD_BUILDNUMBER+"-"+$env:BUILD_SOURCEBRANCHNAME
}
Write-Host "##vso[build.updatebuildnumber]$version"
$script = ".\build.ps1";
$arguments = "buildVersion `"$version`"";
$ScriptBlock = [ScriptBlock]::Create("$script -ScriptArgs '-buildVersion=`"$version`"'")
Invoke-Command -ScriptBlock $ScriptBlock
