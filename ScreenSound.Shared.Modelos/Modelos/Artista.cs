using System.IO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ScreenSound.Modelos;

public class Artista 
{
    public Artista() {
    }

    public Artista(string nome, string bio)
    {
        this.Nome = nome;
        this.Bio = bio;
        this.FotoPerfil = "https://cdn.pixabay.com/photo/2016/08/08/09/17/avatar-1577909_1280.png";
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public string FotoPerfil { get; set; }
    public string Bio { get; set; }

    public virtual List<Musica> Musicas { get; set; } = new List<Musica>();

    public void AdicionarMusica(Musica musica)
    {
        Musicas.Add(musica);
    }

    public void ExibirDiscografia()
    {
        Console.WriteLine($"Discografia do artista {Nome}");
        foreach (var musica in Musicas)
        {
            Console.WriteLine($"Música: {musica.Nome} - Ano Lancamento: {musica.AnoLancamento}");
        }
    }

    public override string ToString()
    {
        return $@"Id: {Id}
            Nome: {Nome}
            Foto de Perfil: {FotoPerfil}
            Bio: {Bio}";
    }
}