using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Service
{
    public class MusicasApi
    {
        private readonly HttpClient _httpClient;
        private readonly string routeName = "musicas";

        public MusicasApi(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ICollection<MusicaResponse>?> GetMusicasAsync()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<MusicaResponse>>(routeName);
        }

        public async Task AddMusicaAsync(MusicaRequest musica)
        {
            await _httpClient.PostAsJsonAsync(routeName, musica);
        }

        public async Task DeletarMusicaAsync(int id)
        {
            await _httpClient.DeleteAsync($"{routeName}/{id}");
        }

        public async Task<MusicaResponse?> GetMusicaPorNomeAsync(string nome)
        {
            return await _httpClient.GetFromJsonAsync<MusicaResponse>($"{routeName}/{nome}");
        }

        public async Task UpdateMusicaAsync(MusicaRequestEdit musica)
        {
            await _httpClient.PutAsJsonAsync(routeName, musica);
        }
    }
}
