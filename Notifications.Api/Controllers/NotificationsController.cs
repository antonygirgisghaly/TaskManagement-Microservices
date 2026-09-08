using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Api.Authorization;
using Notifications.Application.Dtos;
using Notifications.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Notifications.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return Guid.Parse(userIdClaim!);
        }
        //So why we don't need to use [Authorize] on Create endpoint bec. the create endpoint
        //is used by task.api the system itself not user service-to-service Authontication so we implment Api key Authontication to use it to make
        //task.api able to call this endpoint without user authentication.
        [RequireApiKey]
        [HttpPost]
        public async Task<ActionResult<NotificationResponseDto>> Create([FromBody] CreateNotificationRequestDto request, CancellationToken ct)
        {
            var result = await _notificationService.CreateAsync(request, ct);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<NotificationResponseDto>>> GetMyNotifications(CancellationToken ct)
        {
            var result = await _notificationService.GetMyNotificationsAsync(GetUserId(), ct);
            return Ok(result.Data);
        }

        [Authorize]
        [HttpPatch("{id}/read")]
        public async Task<ActionResult> MarkAsRead(Guid id, CancellationToken ct)
        {
            var result = await _notificationService.MarkAsReadAsync(id, GetUserId(), ct);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
        {
            var result = await _notificationService.DeleteAsync(id, GetUserId(), ct);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return NoContent();
        }
    }
}
