using System.Net.Http.Json;
using SmartPdM.Models;

namespace SmartPdM.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        public ApiClient(HttpClient http) => _http = http;


        public async Task<List<DashboardItem>> GetDashboardSummaryAsync(CancellationToken ct = default)
        {
            try
            {
                var items = await _http.GetFromJsonAsync<List<DashboardItem>>("api/dashboard/summary", ct);
                return items ?? new List<DashboardItem>();
            }
            catch
            {
                return new List<DashboardItem>();
            }
        }


        public async Task<bool> IngestRobotMetricsAsync(IEnumerable<object> payload, CancellationToken ct = default)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/robot-metrics/ingest", payload, ct);
                return res.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}