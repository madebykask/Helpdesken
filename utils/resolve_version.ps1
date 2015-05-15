Param ([string]$build_num)

$git_version = (git describe --tags --long --match DEV_?.?.?.? | Select-String -pattern '(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<patch>[0-9]+)\.(?<nothing>[0-9]+)-(?<commitCount>[0-9]+)-(?<hash>[a-z0-9]+)').Matches[0].Groups

$majorVersion = $git_version['major']
$minorVersion = $git_version['minor']
$patchVersion = $git_version['patch']
$commitCount = $git_version['commitCount']
 
$version = [string]::Join('.', @(
	$majorVersion,
	$minorVersion,
	$patchVersion,
	$commitCount
))
 
Write-Host "##teamcity[setParameter name='MajorVersion' value='$majorVersion']"
Write-Host "##teamcity[setParameter name='MinorVersion' value='$minorVersion']"
Write-Host "##teamcity[setParameter name='PatchVersion' value='$patchVersion']"
Write-Host "##teamcity[setParameter name='CommitCount' value='$commitCount']"
Write-Host "##teamcity[buildNumber '$version']"