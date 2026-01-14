using Distro.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Infra / IoC
builder.Services.AddInfrastructureIoC(builder.Configuration);

// OpenAPI (gera o JSON)
builder.Services.AddOpenApi();

// Swagger UI
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // JSON OpenAPI
    app.MapOpenApi();

    // Interface Swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Distro API v1");
        // options.RoutePrefix = "swagger"; 
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
