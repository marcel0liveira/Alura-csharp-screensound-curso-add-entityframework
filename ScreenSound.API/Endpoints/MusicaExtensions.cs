using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ScreenSound.API.Endpoints
{
    public static class MusicaExtensions
    {
        public static void AddEndPointsMusicas(this WebApplication app)
        {
            #region Musicas
            app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
            {
                var musicas = dal.Listar();

                if (musicas is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(EntityListToResponseList(musicas));
            });

            app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
            {
                var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
                if (musica is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(EntityToResponse(musica));
            });

            app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromServices] DAL<Genero> dalGenero, [FromBody] MusicaRequest musicaRequest) =>
            {
                if (musicaRequest is null)
                {
                    return Results.NotFound();
                }

                var musica = new Musica(musicaRequest.Nome)
                {
                    AnoLancamento = musicaRequest.AnoLancamento,
                    ArtistaId = musicaRequest.ArtistaId,
                    Generos = musicaRequest.Generos is not null ? GeneroRequestConverter(musicaRequest.Generos, dalGenero) : new List<Genero>()
                };
                dal.Adicionar(musica);
                return Results.Ok();
            });

            app.MapDelete("/Musicas/{id}", ([FromServices] DAL<Musica> dal, int id) =>
            {
                var musica = dal.RecuperarPor(a => a.Id == id);
                if (musica is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(musica);
                return Results.NoContent();

            });

            app.MapPut("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] Musica musica) =>
            {
                var musicaAAtualizar = dal.RecuperarPor(a => a.Id == musica.Id);
                if (musicaAAtualizar is null)
                {
                    return Results.NotFound();
                }

                // atualiza nome
                musicaAAtualizar.Nome = musica.Nome;
                // atualiza ano lancamento
                musicaAAtualizar.AnoLancamento = musica.AnoLancamento;
                // atualiza artista
                musicaAAtualizar.ArtistaId = musica.ArtistaId;
                musicaAAtualizar.Generos = musica.Generos.Select(a=> 
                        new Genero()
                        {
                            Id = a.Id,
                            Nome = a.Nome,
                            Descricao = a.Descricao
                        }
                    ).ToList();

                dal.Atualizar(musicaAAtualizar);
                return Results.Ok();
            });
            #endregion
        }

        private static ICollection<Genero> GeneroRequestConverter(ICollection<GeneroRequest> generos, DAL<Genero> dalGenero)
        {
            //return generosRequest.Select(a => {
            //    return new Genero()
            //    {
            //        Nome = a.Nome,
            //        Descricao = a.Descricao
            //    };
            //}).ToList();

            var listaDeGeneros = new List<Genero>();
            foreach (var item in generos)
            {
                var entity = new Genero()
                {
                    Nome = item.Nome,
                    Descricao = item.Descricao
                };

                var genero = dalGenero.RecuperarPor(g => g.Nome.ToUpper().Equals(item.Nome.ToUpper()));
                if (genero is not null)
                {
                    listaDeGeneros.Add(genero);
                }
                else
                {
                    listaDeGeneros.Add(entity);
                }
            }

            return listaDeGeneros;
        }

        private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
        {
            return musicaList.Select(a => EntityToResponse(a)).ToList();
        }

        private static MusicaResponse EntityToResponse(Musica musica)
        {
            IEnumerable<GeneroResponse>? generos = null;

            if (musica!.Generos is not null)
            {
                generos = musica!.Generos!.Select(
                        g => new GeneroResponse(g.Id, g.Nome, g.Descricao)
                    ).ToList();
            }

            return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome, musica.AnoLancamento, (ICollection<GeneroResponse>?)generos);
        }
    }
}
