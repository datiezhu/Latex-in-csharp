#
# NugetPackAndPush.ps1
#
Copy-Item .\src\Moosetrail.LaTeX\bin\Release\* .\dist\lib
 nuget pack .\dist\LatexToCSharp.nuspec -OutputDirectory .\dist\
 nuget push .\dist\*.nupkg -s http://moosetrail-nuget.azurewebsites.net/ EA78g0vxKOZe5kS