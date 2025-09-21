clear


# dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
# dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
# dotnet add package Microsoft.AspNetCore.OpenApi
# dotnet add package Microsoft.EntityFrameworkCore.Design
# dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# dotnet add package Microsoft.EntityFrameworkCore.Tools
# dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
# dotnet add package Swashbuckle.AspNetCore

# dotnet aspnet-codegenerator controller \
# -name DistrosController                \
# -async                                 \
# -api                                   \
# -m Distro                              \
# -dc AppDbContext                       \
# -outDir Controllers

dotnet aspnet-codegenerator controller \
-name CategoriasController             \
-async                                 \
-api                                   \
-m Categoria                           \
-dc AppDbContext                       \
-outDir Controllers
