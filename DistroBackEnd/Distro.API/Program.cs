using Distro.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Infra / IoC
builder.Services.AddInfrastructureIoC(builder.Configuration);

// JWT Authentication
builder.Services.AddJwtConfiguration(builder.Configuration);

// Swagger
builder.Services.AddInfrastructureSwagger(builder.Configuration);




// Swagger UI
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();     
    app.UseSwaggerUi();   
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
