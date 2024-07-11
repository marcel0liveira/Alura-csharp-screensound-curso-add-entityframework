using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Runtime.CompilerServices;

namespace ScreenSound.API.Endpoints
{
    public static class ArtistaExtensions
    {
        public static void AddEndPointsArtistas(this WebApplication app)
        {
            #region Artista
            app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
            {
                var artistas = dal.Listar();

                if (artistas.Count() == 0)
                {
                    return Results.NotFound();
                }
                return Results.Ok(EntityListToResponseList(artistas));
            });

            app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
            {
                var artista = dal.RecuperarPor(x => x.Nome.ToUpper().Equals(nome.ToUpper()));

                if (artista is null)
                {
                    return Results.NotFound();
                }

                // string retorno = JsonConvert.SerializeObject(artista);
                return Results.Ok(EntityToResponse(artista));
            });

            app.MapPost("/Artistas", async ([FromServices] IHostEnvironment env,[FromServices] DAL<Artista> dal,[FromBody] ArtistaRequest artistaRequest) =>
            {
                if (artistaRequest is null)
                {
                    return Results.NotFound();
                }

                var nome = artistaRequest.nome.Trim();
                var imagemArtista = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.{nome}.jpg";
                var path = Path.Combine(env.ContentRootPath, "wwwroot", "FotosPerfil", imagemArtista);

                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(artistaRequest.fotoPerfil!));
                using FileStream fs = new(path, FileMode.Create);
                await ms.CopyToAsync(fs);

                var artista = new Artista(artistaRequest.nome, artistaRequest.bio)
                {
                    FotoPerfil = $"/FotosPerfil/{imagemArtista}"
                };

                dal.Adicionar(artista);
                return Results.Ok();
            });

            app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) =>
            {
                var artistaAAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);
                if (artistaAAtualizar is null)
                {
                    return Results.NotFound();
                }

                artistaAAtualizar.Nome = artista.Nome;
                artistaAAtualizar.Bio = artista.Bio;
                artistaAAtualizar.FotoPerfil = artista.FotoPerfil;
                dal.Atualizar(artistaAAtualizar);
                return Results.Ok();
            });

            app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
            {
                var artista = dal.RecuperarPor(a => a.Id == id);

                if (artista is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(artista);
                return Results.NoContent();
            });
            #endregion
        }

        private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
        {
            return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
        }

        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
        }
    }
}
