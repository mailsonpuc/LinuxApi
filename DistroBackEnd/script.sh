
# aply initial migration
dotnet ef migrations add Inicial --project Distro.Infra.Data --startup-project Distro.API


# aply database migrations
dotnet ef database update  --project Distro.Infra.Data --startup-project Distro.API
