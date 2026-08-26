namespace CinemaManagement.DTOs
{
    public class FilmeUpdateDto
    {
        public string Titulo { get; set; } = string.Empty;

        public string Sinopse { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;

        public int DuracaoMinutos { get; set; }

        public string ClassificacaoIndicativa { get; set; } = string.Empty;

        public string PosterUrl { get; set; } = string.Empty;

        public DateTime DataEstreia { get; set; }

        public bool Ativo { get; set; }
    }
}
