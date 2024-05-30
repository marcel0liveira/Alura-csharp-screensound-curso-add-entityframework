using ScreenSound.Web.Pages;
using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Service
{
    public class ArtistasAPI
    {
        private readonly HttpClient _httpClient;

        public ArtistasAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("artistas");
        }

        public async Task AddArtistaAsync(ArtistaRequest artista)
        {
            await _httpClient.PostAsJsonAsync("artistas", artista);
        }

        public async Task DeletarArtistaAsync(int id)
        {
            await _httpClient.DeleteAsync($"artista/{id}");
        }

        public async Task<ArtistaResponse?> GetArtistaPorNomeAsync(string nome)
        {
            return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistas/{nome}");
        }
    }
}
