namespace CinemaManagement.DTOs
{
    public class SalaUpdateDto
    {
        public string Nome { get; set; } = string.Empty;

        public int Capacidade { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public bool Ativa { get; set; }
    }
}
