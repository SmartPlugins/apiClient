set /p apiKey=NuGet API Key: 
set /p version=Package Version: 

nuget.exe push Packages/SmartPlugin.ApiClient.%version%.nupkg %apiKey%
nuget.exe push Packages/SmartPlugin.ApiClient.HttpClient.%version%.nupkg %apiKey%