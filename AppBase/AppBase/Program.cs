using AppBase.Notification;
using AppBase.Auth.Token; // Assure-toi d'importer le bon namespace
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration CORS pour autoriser toutes les origines
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Autoriser toutes les origines
            .AllowAnyHeader()   // Autoriser tous les en-têtes
            .AllowAnyMethod();  // Autoriser toutes les méthodes HTTP
    });
});

// Configuration JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

// Enregistrement de JwtService dans le conteneur DI
builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtSettings.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();

// Ajout des services nécessaires
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WebSocketService>(); // Injection du service WebSocket

var app = builder.Build();

// Activation de Swagger pour la documentation
app.UseSwagger();
app.UseSwaggerUI();

// Activer les WebSockets
app.UseWebSockets();

// Activation de CORS
app.UseCors("AllowAll");

// Activation de l'authentification et de l'autorisation JWT
app.UseAuthentication();
app.UseAuthorization();

// Mapping des routes API
app.MapControllers();

// Gestion des connexions WebSocket sur l'endpoint "/ws"
app.Map("/ws", async (context) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var webSocketService = context.RequestServices.GetRequiredService<WebSocketService>();
        await webSocketService.HandleWebSocketAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.Run();
