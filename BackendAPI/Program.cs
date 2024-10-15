using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BackendAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        if (environment != "Development")
        {
            // If component runs in Azure, then use App Registration to authenticate
            var tenantId = Environment.GetEnvironmentVariable("TenantId");
            var audienceUri = "api://nis2024-demo";

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://login.microsoftonline.com/{tenantId}";
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://sts.windows.net/{tenantId}",

                        ValidateAudience = true,
                        ValidAudience = audienceUri,

                        ValidateLifetime = true,
                    };
                });
        }
        else
        {
            // If component runs on develop machine, then use mock authentication
            builder.Services.AddAuthentication("Development")
                .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>("Development", options => { });
        }


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

