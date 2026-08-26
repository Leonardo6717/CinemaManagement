namespace CinemaManagement.DTOs
{
    public class SessaoUpdateDto
    {
        public int FilmeId { get; set; }

        public int SalaId { get; set; }

        public DateTime DataHora { get; set; }

        public decimal PrecoIngresso { get; set; }

        public bool Ativa { get; set; }
    }
}
