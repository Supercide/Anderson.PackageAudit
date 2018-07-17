$version = "0.11.1"

if(!($Env:BUILD_SOURCEBRANCHNAME -eq "master"))
{
    $version = "$version."+$Env:BUILD_BUILDNUMBER+"-"+$Env:BUILD_SOURCEBRANCHNAME
}
Write-Host "##vso[build.updatebuildnumber]$version"
$script = ".\build.ps1";
$arguments = "buildVersion `"$version`"";
$ScriptBlock = [ScriptBlock]::Create("$script -ScriptArgs '-buildVersion=`"$version`"'")
Invoke-Command -ScriptBlock $ScriptBlock