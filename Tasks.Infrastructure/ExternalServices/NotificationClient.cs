using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using Tasks.Application.Interfaces;

namespace Tasks.Infrastructure.ExternalServices
{
    public class NotificationClient : INotificationClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotificationClient> _logger;

        public NotificationClient(HttpClient httpClient, ILogger<NotificationClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task NotifyAsync(Guid userId, string message, CancellationToken ct = default)
        {
            var payload = new
            {
                UserId = userId,
                Message = message
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/notifications", payload, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(ct);
                    _logger.LogError("Notification call failed: {StatusCode} - {Body}", response.StatusCode, errorBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification.");
            }
        }
    }
}
