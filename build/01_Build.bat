nuget restore ../src/SmartPlugin.ApiClient.sln
msbuild ../src/SmartPlugin.ApiClient.sln /p:Configuration=Release /t:rebuild
dotnet publish --configuration "Release" --output "../../Publish" "../src"