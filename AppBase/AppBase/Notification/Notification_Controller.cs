using Microsoft.AspNetCore.Mvc;

namespace AppBase.Notification;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly WebSocketService _webSocketService;

    public NotificationController(WebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        await _webSocketService.SendNotificationAsync(request.Message);
        return Ok(new { Message = "Notification envoyée avec succès" });
    }
}

public class NotificationRequest
{
    public string Message { get; set; }
}