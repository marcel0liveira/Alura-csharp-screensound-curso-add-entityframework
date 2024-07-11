using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Service
{
    public class ArtistasAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string routeName = "artistas";

        public ArtistasAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>(routeName);
        }

        public async Task AddArtistaAsync(ArtistaRequest artista)
        {
            await _httpClient.PostAsJsonAsync(routeName, artista);
        }

        public async Task DeletarArtistaAsync(int id)
        {
            await _httpClient.DeleteAsync($"{routeName}/{id}");
        }

        public async Task<ArtistaResponse?> GetArtistaPorNomeAsync(string nome)
        {
            return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"{routeName}/{nome}");
        }

        public async Task UpdateArtistaAsync(ArtistaRequestEdit artista)
        {
            await _httpClient.PutAsJsonAsync(routeName, artista);
        }
    }
}
