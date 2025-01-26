using AppBase.Notification;

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