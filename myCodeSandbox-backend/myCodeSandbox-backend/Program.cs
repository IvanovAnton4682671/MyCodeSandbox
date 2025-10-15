var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddScoped<ICodeService, CodeService>();
builder.Services.AddScoped<IDockerService, DockerService>();

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseRouting();
app.MapControllers();
app.Run();