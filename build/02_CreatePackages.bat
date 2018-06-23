nuget pack ../src/SmartPlugin.ApiClient/SmartPlugin.ApiClient.csproj -IncludeReferencedProjects -OutputDirectory "Packages" -Prop Configuration=Release
nuget pack ../src/SmartPlugin.ApiClient.HttpClient/SmartPlugin.ApiClient.HttpClient.csproj -IncludeReferencedProjects -OutputDirectory "Packages" -Prop Configuration=Release
