using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatAPI.Models;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.User, message.Text);
        return Ok(new { Message = "Message sent successfully." });
    }
}
