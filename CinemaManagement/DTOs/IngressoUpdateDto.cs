namespace CinemaManagement.DTOs
{
    public class IngressoUpdateDto
    {
        public int ClienteId { get; set; }

        public int SessaoId { get; set; }

        public string Assento { get; set; } = string.Empty;

        public decimal PrecoPago { get; set; }

        public bool Ativo { get; set; }
    }
}
