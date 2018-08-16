using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Anderson.PackageAudit.Audit.Auditors.Sonatype
{
    public class Client : ISonatypeClient
    {
        private readonly HttpClient _httpClient;

        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Report[]> GetReportAsync(string[] coordinates)
        {
            var componentReportRequest = new ComponentReportRequest
            {
                coordinates = coordinates
            };

            using (var response = await _httpClient.PostAsJsonAsync("api/v3/component-report", componentReportRequest))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Report[]>(json);
                }

                throw new HttpRequestException();
            }
        }
    }
}