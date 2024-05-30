using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Service
{
    public class MusicasApi
    {
        private readonly HttpClient _httpClient;

        public MusicasApi(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ICollection<MusicaResponse>?> GetMusicasAsync()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<MusicaResponse>>("musicas");
        }
    }
}
