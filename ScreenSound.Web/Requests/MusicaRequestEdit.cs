using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Web.Requests
{
    public record MusicaRequestEdit (
        int id,
        [Required] string Nome, 
        [Required] int ArtistaId, 
        int AnoLancamento,
        ICollection<GeneroRequestEdit>? Generos=null)
    {
    }
}
