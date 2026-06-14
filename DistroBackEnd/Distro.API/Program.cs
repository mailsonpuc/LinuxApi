using Distro.API.Middleware;
using Distro.Infra.IoC;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Infra / IoC
builder.Services.AddInfrastructureIoC(builder.Configuration);

// JWT Authentication
builder.Services.AddJwtConfiguration(builder.Configuration);

// Swagger
builder.Services.AddInfrastructureSwagger(builder.Configuration);


// Rate Limiter
builder.Services.AddInfrastructureRateLimiter(builder.Configuration);


//  CORS
builder.Services.AddInfrastructureCors(builder.Configuration);


var app = builder.Build();

app.UseGlobalExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();

    app.UseSwaggerUi(options =>
    {
        options.Path = "";
    });
}


app.UseHttpsRedirection();


app.UseCors("AllowAll");

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();