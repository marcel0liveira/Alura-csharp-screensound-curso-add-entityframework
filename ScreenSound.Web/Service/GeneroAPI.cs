using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Service
{
    public class GeneroAPI
    {
        private readonly HttpClient _httpClient;
        private readonly string routeName = "generos";

        public GeneroAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ICollection<GeneroResponse>?> GetGenerosAsync()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<GeneroResponse>>(routeName);
        }

        public async Task AddGeneroAsync(GeneroRequest genero)
        {
            await _httpClient.PostAsJsonAsync(routeName, genero);
        }

        public async Task DeletarGeneroAsync(int id)
        {
            await _httpClient.DeleteAsync($"{routeName}/{id}");
        }

        public async Task<GeneroResponse?> GetGeneroPorNomeAsync(string nome)
        {
            return await _httpClient.GetFromJsonAsync<GeneroResponse>($"{routeName}/{nome}");
        }

        public async Task UpdateGeneroAsync(GeneroRequestEdit genero)
        {
            await _httpClient.PutAsJsonAsync($"{routeName}", genero);
        }
    }
}
