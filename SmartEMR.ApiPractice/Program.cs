using Microsoft.EntityFrameworkCore;
using SmartEMR.ApiPractice.Infrastructure;
using SmartEMR.ApiPractice.Repository;
using SmartEMR.ApiPractice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()   // 모든 도메인 허용 (개발용)
              .AllowAnyMethod()   // GET, POST 등 모든 메서드 허용
              .AllowAnyHeader();  // 모든 헤더 허용
    });
});

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPatientService, PatientService>();
builder.Services.AddSingleton<IPatientRepository, PatientRepository>();

builder.Services.AddSingleton<IReservationService, ReservationService>();
builder.Services.AddSingleton<IReservationRepository, ReservationRepository>();

SetDBSettings(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void SetDBSettings(WebApplicationBuilder builder)
{
    var dbSettings = builder.Configuration.GetSection("DBSettings").Get<DBSettings>();
    if (dbSettings is null) 
        return;

    string connectionStrings = $"Server={dbSettings.Server};" +
                               $"Database={dbSettings.Database};" +
                               $"Trusted_Connection={dbSettings.TrustedConnection};" +
                               $"TrustServerCertificate={dbSettings.TrustServerCertificate};";

    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionStrings));
}