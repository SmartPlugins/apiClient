@REM oy2j6o7y2asoqhiit44aqrjlphf24ise3lmuaumhwfo5wu
set /p apiKey=NuGet API Key: 
set /p version=Package Version: 

REM nuget.exe push Packages/SmartPlugin.ApiClient.%version%.nupkg %apiKey%
REM nuget.exe push Packages/SmartPlugin.ApiClient.HttpClient.%version%.nupkg %apiKey%
dotnet nuget push ../Publish/nupkgs/SmartPlugin.ApiClient.%version%.nupkg -k %apiKey% -s https://api.nuget.org/v3/index.json
dotnet nuget push ../Publish/nupkgs/SmartPlugin.ApiClient.HttpClient.%version%.nupkg -k %apiKey% -s https://api.nuget.org/v3/index.json
dotnet nuget push ../Publish/nupkgs/SmartPlugin.ApiClient.CodeGen.%version%.nupkg -k %apiKey% -s https://api.nuget.org/v3/index.json