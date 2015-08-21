Since version 5.3.12 we using changed process in versioninig

now it will be like 5.3.12-1-b347684
where 
- 5.3.12 Major, minor, patch version;
- 1 is a commit count since this version started;
- b347684 is short commit hash used to build this version (usefull to put in search GitExtensions field);

Why we should use it?
GIT gives us automatic version generation based on information it holds.
(https://www.kernel.org/pub/software/scm/git/docs/git-describe.html)
So to spare from boring work of version enumeration we will use automatic version generator script.

In utils folder you see files:
GenerateAssemblyInfo.xml - msbuild target script that does all work
GenerateAssemblyInfo.bat - shortcut to MSBuild with GenerateAssemblyInfo.xml. 
resolve_version.ps - reserverd for Teamcity
deploy.xml - MSBUild file contans targets to deploy on DEV and ACCT. Do not use it now - Work still in progress.

GenerateAssemblyInfo.xml calls "git describe --tags" and pareses output. Then it generates content to file that was supplied in "AssemblyFileName" option or "..\dh\dhHelpdeskNG\SharedAssemblyInfo.cs" by default. So it is mandatory to use tags in vX.X.X format, otherwise you should change regexp that is suppling to git describe command, and regexp that parse and generate version to C# file template in GenerateAssemblyInfo.xml 

There are dependency in VS solution now that calls GenerateAssemblyInfo.bat when developer builds "Release" target. So when you build "release" VS calls GenerateAssemblyInfo.bat file and it calls MSBuild.exe that overwrites SharedAsemblyInfo.cs file. So VS2012 will ask you that some files was changed should it be updated?. Dont be confused with this and agree.

In debug target the BAT file is not called. So version will not change (as it was before).


1. How to start new version?
Create tag like v5.3.12 with "Put tag to 'origin'" checkbox checked in GitExtensions

2. How to build Release
as usual, when VS ask you to update changed files after build do agree.