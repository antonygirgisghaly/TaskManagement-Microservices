using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Tasks.Application.Dtos;
using Tasks.Application.Interfaces;

namespace Tasks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return Guid.Parse(userIdClaim!);
        }
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create([FromBody] CreateTaskRequestDto request,CancellationToken ct = default)
        {
            var userId = GetUserId();
            var result = await _taskService.CreateAsync(request, userId, ct);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }
        [HttpGet]
        public async Task<ActionResult<List<TaskResponseDto>>> GetMyTasks(CancellationToken ct = default)
        {
            var userId = GetUserId();
            var result = await _taskService.GetMyTasksAsync(userId, ct);
            if (!result.IsSuccess)
                return NotFound("No tasks found.");
            return Ok(result.Data);
        }
        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(Guid taskId, CancellationToken ct = default)
        {
            var userId = GetUserId();
            var result = await _taskService.GetTaskByIdAsync(taskId, userId, ct);
            if (!result.IsSuccess)
                return NotFound(result.Error);
            return Ok(result.Data);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponseDto>> Update(Guid id, [FromBody] UpdateTaskRequestDto request, CancellationToken ct)
        {
            var result = await _taskService.UpdateAsync(id, request, GetUserId(), ct);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TaskResponseDto>> UpdateStatus(Guid id, [FromBody] UpdateTaskStatusRequestDto request, CancellationToken ct)
        {
            var result = await _taskService.UpdateStatusAsync(id, request, GetUserId(), ct);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return Ok(result.Data);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
        {
            var result = await _taskService.DeleteAsync(id, GetUserId(), ct);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Error });

            return NoContent();
        }
    }
}
