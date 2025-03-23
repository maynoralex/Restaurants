using Microsoft.OpenApi.Models;
using Restaurants.API.Middlewares;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPrestentation(this WebApplicationBuilder builder){
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c => {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme{
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"        
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    }, []
                }
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLogMiddleware>();
        builder.Host.UseSerilog((context, configuration) => {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

    }
}
