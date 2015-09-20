#
# NugetPackAndPush.ps1
#

$file = "C:\Users\Johanna\Documents\MoosetrailLatexToCSharp\dist\LatexToCSharp.nuspec"
$matches = Get-Content $file -ErrorAction SilentlyContinue |
    Select-String '\<version\>(\d.\d.\d)\</version\>'

"The last version is " + $matches[0].Matches.Groups[1].Value

$setNewVersion = Read-Host -Prompt 'do you want to create a new version (y or n)?'

if($setNewVersion -eq 'y'){

$newFileVersion = Read-Host -Prompt "What do you want the new version to be? (Old: $matches )"

$toReplace = "<version>" + $matches[0].Matches.Groups[1].Value + "</version>"
(Get-Content $file) | Foreach-Object {

    $_ -replace $toReplace, "<version>$newFileVersion</version>" `
    } | Set-Content $file
}

Remove-Item C:\Users\Johanna\Documents\MoosetrailLatexToCSharp\dist\*.nupkg

Copy-Item .\src\Moosetrail.LaTeX\bin\Release\* .\dist\lib
 nuget pack .\dist\LatexToCSharp.nuspec -OutputDirectory .\dist\
 nuget push .\dist\*.nupkg -s http://moosetrail-nuget.azurewebsites.net/ EA78g0vxKOZe5kS