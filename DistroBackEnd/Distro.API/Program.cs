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


//  CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();