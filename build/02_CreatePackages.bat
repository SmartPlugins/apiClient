@REM nuget pack ../src/SmartPlugin.ApiClient/SmartPlugin.ApiClient.csproj -IncludeReferencedProjects -OutputDirectory "Packages" -Prop Configuration=Release
@REM nuget pack ../src/SmartPlugin.ApiClient.HttpClient/SmartPlugin.ApiClient.HttpClient.csproj -IncludeReferencedProjects -OutputDirectory "Packages" -Prop Configuration=Release

dotnet pack  --configuration "Release" --output "../../Publish/nupkgs" "../src"
