﻿using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints;

public static class GeneroExtensions
{
    public static void AddEndPointGeneros(this WebApplication app)
    {
        app.MapGet("/Generos", ([FromServices] DAL<Genero> dal) =>
        {
            var generos = dal.Listar();

            if (generos is not null)
            {
                return Results.Ok(EntityListToResponseList(generos));
            }
            return Results.NotFound("Gênero não encontrado.");
        });

        app.MapGet("/Generos/{nome}", ([FromServices] DAL<Genero> dal, string nome) =>
        {
            var genero = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            if (genero is not null)
            {
                var response = EntityToResponse(genero!);
                return Results.Ok(response);
            }
            return Results.NotFound("Gênero não encontrado.");
        });

        app.MapPost("/Generos", ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequest generoReq) =>
        {
            dal.Adicionar(RequestToEntity(generoReq));
        });

        app.MapDelete("/Generos/{id}", ([FromServices] DAL<Genero> dal, int id) =>
        {
            var genero = dal.RecuperarPor(a => a.Id == id);
            if (genero is null)
            {
                return Results.NotFound("Gênero para exclusão não encontrado.");
            }
            dal.Deletar(genero);
            return Results.NoContent();
        });
    }

    private static Genero RequestToEntity(GeneroRequest generoRequest)
    {
        return new Genero() { Nome = generoRequest.Nome, Descricao = generoRequest.Descricao };
    }

    private static ICollection<GeneroResponse> EntityListToResponseList(IEnumerable<Genero> generos)
    {
        return generos.Select(a => EntityToResponse(a)).ToList();
    }

    private static GeneroResponse EntityToResponse(Genero genero)
    {
        return new GeneroResponse(genero.Id, genero.Nome!, genero.Descricao!);
    }
}