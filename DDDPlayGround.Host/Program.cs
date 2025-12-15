using DDDPlayGround.Host;
using DDDPlayGround.Infrastructure;
using DDDPlayGround.Infrastructure.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConnectionString(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerAuthentication();

builder.Services.AddJwtAuthentication(builder.Configuration); 

builder.Services.AddDependancyInjection();

// Register SSO Service for token validation
builder.Services.AddScoped<DDDPlayGround.Infrastructure.Services.SSOService>();

builder.Host.AddLoggingService(builder.Configuration);
  
builder.Services.AddMapperService();

builder.Services.AddCorsService();

builder.Services.AddRateLimiting();

builder.Services.AddHttpClient();

builder.Services.AddHttpAccessor();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddFluentValidationServices();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
    });
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

var app = builder.Build();

app.UseCustomSerilogRequestLogging();

app.UseSwagger();

app.UseCors("AllowAll");

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DDDPlayGround API V1");
});

app.UseHttpsRedirection();

if (builder.Environment.IsProduction()) { app.UseHsts(); } // this allow only https request 

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomExceptionMiddleware();

app.UseRateLimiter();

app.UseRequestLogging();

app.MapControllers();

app.Run();